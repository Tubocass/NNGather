using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using System.Collections;

public class Unit_Base : NetworkBehaviour 
{
	public static int TotalCreated;
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
				if(health<=0)
				{
					Death();
				}
			}
		}
	}
	public float startHealth;
	public MoMController myMoM;
	[SyncVar(hook = "SetHealthUI")][SerializeField] protected float health; 
	[SyncVar]protected bool hasChanged;
	[SerializeField]protected float MaxHoverDistance = 20, MinHoverDistance = 1;
	protected Transform tran;//component
	protected NavMeshAgent agent;//component
	protected Material TeamColorMat;//value in a component
	protected Vector3 currentVector = Vector3.zero;
	protected Vector3[] Path;
	protected bool bMoving;
	protected int points = 0, currntPoint = 0;
	protected bool bDay;
	int pathAttempts;
	float maxDistanceSqrd, minDistanceSqrd;

	protected virtual void OnEnable () 
	{
		health = startHealth;
		agent = GetComponent<NavMeshAgent>();
		tran = transform;
		TeamColorMat = GetComponentInChildren<MeshRenderer>().material;
	}
	protected virtual void OnDisable()
	{
		StopAllCoroutines();
	}
	protected virtual void Start() 
	{
		maxDistanceSqrd = MaxHoverDistance*MaxHoverDistance;
		minDistanceSqrd = MinHoverDistance*MinHoverDistance;
		unitID = TotalCreated;
		TotalCreated+=1;
	}
//	protected virtual void DaySwitch(bool b)
//	{
//		bDay = b;
//	}
	//[ClientRpc]
	public virtual void SetMoM(GameObject mom)
	{
		isActive = true;
		myMoM = mom.GetComponent<MoMController>();
		teamID = myMoM.teamID;
		TeamColor = myMoM.TeamColor;
	}
	protected void OnChangeColor(Color newColor)
	{
		TeamColor = newColor;
		TeamColorMat.color = newColor;
		hasChanged = true;
	}
	public override void OnStartClient()
	{
		if(hasChanged)
		TeamColorMat.color = TeamColor;
	}
	//[ClientRpc]
	public void SetHealthUI(float h)
	{
		health = h;
		if(isLocalPlayer && this.GetType()==typeof(PlayerMomController))
		UnityEventManager.TriggerEvent("UpdateHealth", (int)health);
	}

	protected virtual void Death()
	{
		UnityEventManager.TriggerEvent("TargetUnavailable",unitID);
		bMoving = false;
		isActive = false;
		hasChanged = false;
		if(teamID>=0&& NewGameController.Instance.TeamSize[teamID]>0)
            NewGameController.Instance.TeamSize[teamID]-=1;
	}
	public virtual void TakeDamage(float damage)
	{
		Health = -damage;
	}

	public Vector3 RandomVector(Vector3 origin, float range)
	{
		Vector3 rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
		UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
		agent.CalculatePath(rando,path);
		float distSqrd = (rando-tran.position).sqrMagnitude;
		pathAttempts = 10;
		while(pathAttempts>0 && (distSqrd>maxDistanceSqrd|| distSqrd<minDistanceSqrd) || (path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial))
		{
			pathAttempts--;
			rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
			agent.CalculatePath(rando,path);
			distSqrd = (rando-tran.position).sqrMagnitude;
		}
		return rando;
	}

	[Server]
	public void MoveTo(Vector3 location)
	{
		if(!location.Equals(Vector3.zero))
		{
			currentVector = location; 
			agent.SetDestination(currentVector);
//			NavMeshPath path = new NavMeshPath();
//			agent.CalculatePath(location, path);
//			agent.SetPath(path);
			RpcMoveTo(currentVector);
		}
	}

	[ClientRpc]
	public void RpcMoveTo(Vector3 loc)
	{
		currentVector = loc; 
		agent.SetDestination(currentVector);
	}

	[Server]
	protected virtual void MoveRandomly(Vector3 origin, float distance)
	{
		Vector3 rVector = RandomVector(origin, distance);
		MoveTo(rVector);
	}

	public virtual void OnCollisionEnter(Collision bang)
	{
	}
	public virtual void OnTriggerEnter(Collider other)
	{
	}
}
