using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Unit_Base : NetworkBehaviour 
{
	public static int TotalCreated;
	public static int[] TeamSize = new int[10];
	[SyncVar]public int teamID, unitID;
	[SyncVar(hook = "OnChangeColor")] public Color TeamColor;
	public bool isActive{get{return gameObject.activeSelf;}set{gameObject.SetActive(value); if(value==false)OnDisable();}}
	public Vector3 Location{get{return transform.position;}}
	public float Health{
		get
		{
			return health;
		}
		set
		{
			if(isServer)
			{
				if(health<=0)//already dead, leave me be
				{
					return;
				}
				health+=value; 
				if(this.GetType()==typeof(PlayerMomController))
				{
					RpcSetHealthUI();
					//UnityEventManager.TriggerEvent("UpdateHealth", (int)health);
				}
				if(health<=0)
				{
					Death();
				}
			}
		}
	}
	public MoMController myMoM;
	[SyncVar][SerializeField] protected float health, startHealth;
	[SerializeField] protected float MaxHoverDistance = 20, MinHoverDistance = 1;
	[SerializeField] protected Vector3 currentVector;
	[SerializeField] protected bool bMoving;
	[SerializeField] protected List<Vector3> Path;
	[SerializeField] protected int points, currntPoint;
	[SerializeField] int tries;
	[SyncVar]bool hasChanged;
	protected Transform tran;
	protected UnityEngine.AI.NavMeshAgent agent;
	protected bool bDay;
	protected Material TeamColorMat;
	float maxDistanceSqrd, minDistanceSqrd;

	protected virtual void OnEnable () 
	{
		Path = new List<Vector3>();
		maxDistanceSqrd = MaxHoverDistance*MaxHoverDistance;
		minDistanceSqrd = MinHoverDistance*MinHoverDistance;
		tran = transform;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		currentVector = tran.position;
		health = startHealth;
		unitID = TotalCreated;
		TotalCreated+=1;
		bDay = GameController.IsDayLight();
		TeamColorMat = GetComponentInChildren<MeshRenderer>().material;
		UnityEventManager.StartListeningBool("DayTime", DaySwitch);

	}
	protected virtual void OnDisable()
	{
		UnityEventManager.StopListeningBool("DayTime", DaySwitch);
	}
	protected virtual void DaySwitch(bool b)
	{
		bDay = b;
	}

	public virtual void SetMoM(GameObject mom)
	{
		isActive = true;
		myMoM = mom.GetComponent<MoMController>();
		teamID = myMoM.teamID;
	}
	protected void OnChangeColor(Color newColor)
	{
		if(TeamColorMat!=null)
		TeamColorMat.color = newColor;
		hasChanged = true;
	}
	public override void OnStartClient()
	{
		base.OnStartClient();
		if(hasChanged)
		TeamColorMat.color = TeamColor;
	}

	[ClientRpc]
	public void RpcSetHealthUI()
	{
		UnityEventManager.TriggerEvent("UpdateHealth", (int)health);
	}

	protected virtual void Death()
	{
		UnityEventManager.TriggerEvent("TargetUnavailable",unitID);
		UnityEventManager.StopListeningBool("DayTime", DaySwitch);
		StopAllCoroutines();
		bMoving = false;
		isActive = false;
		hasChanged = false;
		if(teamID>=0&&TeamSize[teamID]>0)
		TeamSize[teamID]-=1;
	}
	public virtual void TakeDamage(float damage)
	{
		Health = -damage;
	}


	public UnityEngine.AI.NavMeshPath RandomPath(Vector3 origin, float range)
	{
		Vector3 rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
		UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
		agent.CalculatePath(rando,path);
		float dist = (rando-tran.position).sqrMagnitude;
		tries = 10;
		while(tries>0 && (dist>maxDistanceSqrd|| dist<minDistanceSqrd) || (path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial))
		{
			tries--;
			rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
			agent.CalculatePath(rando,path);
			dist = (rando-tran.position).sqrMagnitude;
		}
		return path;
	}
	[Server]
	public void MoveTo(Vector3 location)
	{
		NavMeshPath path = new NavMeshPath();
		agent.CalculatePath(location, path);
		RpcMoveTo(path.corners);
	}

	[ClientRpc]
	public void RpcMoveTo(Vector3[] PathArray)
	{
//		StopCoroutine("MovingTo");
		if(PathArray.Length>0)
		{
			if(!bMoving)
			{
				Path.Clear();
				bMoving = true;
				points = PathArray.Length;
				for(int p = 0; p<PathArray.Length; p++)
				{	
					Path.Add(PathArray[p]);
				}
				currntPoint = 0;
				currentVector = Path[currntPoint];
				agent.SetDestination(currentVector);
				StartCoroutine("MovingTo");
			}else
			{
				for(int p = 0; p<PathArray.Length; p++)
				{	
					Path.Add(PathArray[p]);
				}
				points += PathArray.Length;
				currntPoint++;
				currentVector = Path[currntPoint];
				agent.SetDestination(currentVector);
			}

		}
	}

	protected virtual IEnumerator MovingTo()
	{
		while(bMoving)
		{
			if(agent.remainingDistance<1)
			{
				if(currntPoint<points-1)
				{
					currntPoint +=1;
					currentVector = Path[currntPoint];
					agent.SetDestination(currentVector);
				}else bMoving = false;
				//Debug.Log("I arrived");
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
	[Server]
	protected virtual void MoveRandomly(Vector3 origin, float distance)//Vector3[] PathArray
	{
		NavMeshPath rVector = RandomPath(origin, distance);
		RpcMoveTo(rVector.corners);
	}


	public virtual void OnCollisionEnter(Collision bang)
	{
	}
	public virtual void OnTriggerEnter(Collider other)
	{
	}
}
