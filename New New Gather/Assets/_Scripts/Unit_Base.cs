using UnityEngine;
using System.Collections;

public class Unit_Base : MonoBehaviour 
{
	public static int TotalCreated;
	public int teamID, unitID;
	public bool isActive{get{return gameObject.activeSelf;}set{gameObject.SetActive(value);}}
	public Vector3 Location{get{return tran.position;}}
	public float Health{
		get
		{
			return health;
		}
		set
		{
			health+=value; 
			if(this.GetType()==typeof(MainMomController))
			{
				UnityEventManager.TriggerEvent("UpdateHealth", (int)health);
			}
			if(health<=0)
			{
				Death();
			}
		}
	}

	[SerializeField] protected float MaxHoverDistance = 20, MinHoverDistance = 1;
	[SerializeField] protected Vector3 currentVector;
	[SerializeField] protected bool bMoving;
	[SerializeField] protected float health, startHealth;
	[SerializeField] int tries;
	protected MoMController myMoM;
	protected Transform tran;
	protected NavMeshAgent agent;
	float maxDistanceSqrd, minDistanceSqrd;

	protected virtual void OnEnable () 
	{
		maxDistanceSqrd = MaxHoverDistance*MaxHoverDistance;
		minDistanceSqrd = MinHoverDistance*MinHoverDistance;
		tran = transform;
		agent = GetComponent<NavMeshAgent>();
		currentVector = tran.position;
		health = startHealth;
		TotalCreated+=1;
		unitID = TotalCreated;
	}

	public virtual void setMoM(MoMController mom)
	{
		isActive = true;
		myMoM = mom;
		teamID = myMoM.teamID;
		tran.position = mom.Location + new Vector3(1,0,1);
	}

	protected virtual void Death()
	{
		UnityEventManager.TriggerEvent("TargetUnavailable",unitID);
		StopAllCoroutines();
		bMoving = false;
		isActive = false;
	}

	public Vector3 RandomVector(Vector3 origin, float range)
	{
		Vector3 rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
		NavMeshPath path = new NavMeshPath();
		agent.CalculatePath(rando,path);
		float dist = (rando-tran.position).sqrMagnitude;
		tries = 10;
		while(tries>0 && (dist>maxDistanceSqrd|| dist<minDistanceSqrd) || (path.status == NavMeshPathStatus.PathPartial))
		{
			tries--;
			rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
			agent.CalculatePath(rando,path);
			dist = (rando-tran.position).sqrMagnitude;
		}
		return rando;
	}

	public void MoveTo(Vector3 location)
	{
		//agent.ResetPath();
		bMoving = true;
		currentVector = location;
		agent.SetDestination(location);
		StopCoroutine("MovingTo");
		StartCoroutine("MovingTo");
	}

	protected virtual IEnumerator MovingTo()
	{
		while(bMoving)
		{
			if(agent.remainingDistance<1)
			{
				bMoving = false;
				//Debug.Log("I arrived");
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}
