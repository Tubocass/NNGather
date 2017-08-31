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
				if(this.GetType()==typeof(PlayerMomController))
				{
					//RpcSetHealthUI();
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
	[SyncVar(hook = "SetHealthUI")][SerializeField] protected float health; 
	[SyncVar]protected bool hasChanged;
	[SerializeField] protected float startHealth;
	protected float MaxHoverDistance = 20, MinHoverDistance = 1;
	protected Vector3 currentVector;
	protected bool bMoving;
	protected Vector3[] Path;
	protected int points = 0, currntPoint = 0;
	protected Transform tran;
	protected UnityEngine.AI.NavMeshAgent agent;
	protected bool bDay;
	protected Material TeamColorMat;
	int pathAttempts;
	float maxDistanceSqrd, minDistanceSqrd;

	protected virtual void OnEnable () 
	{
		maxDistanceSqrd = MaxHoverDistance*MaxHoverDistance;
		minDistanceSqrd = MinHoverDistance*MinHoverDistance;
		tran = transform;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		currentVector = tran.position;
		health = startHealth;
		TotalCreated+=1;
		unitID = TotalCreated;
		TeamColorMat = GetComponentInChildren<MeshRenderer>().material;
	}
	protected virtual void OnDisable()
	{
		StopAllCoroutines();
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
		if(teamID>=0&&GameController.instance.TeamSize[teamID]>0)
		GameController.instance.TeamSize[teamID]-=1;
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
		float dist = (rando-tran.position).sqrMagnitude;
		pathAttempts = 10;
		while(pathAttempts>0 && (dist>maxDistanceSqrd|| dist<minDistanceSqrd) || (path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial))
		{
			pathAttempts--;
			rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
			agent.CalculatePath(rando,path);
			dist = (rando-tran.position).sqrMagnitude;
		}
		return rando;
	}

	[Server]
	public void MoveTo(Vector3 location)
	{
		if(!location.Equals(Vector3.zero))
		{
//			currentVector = location; 
//			agent.SetDestination(currentVector);
			RpcMoveTo(location);
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
