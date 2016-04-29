using UnityEngine;
using System.Collections;

public class Unit_Base : MonoBehaviour 
{
	public int TeamID;
	public bool isActive{get{return gameObject.activeSelf;}set{gameObject.SetActive(value);}}
	public Vector3 Location{get{return transform.position;}}
	protected Transform tran;
	[SerializeField] protected float MaxHoverDistance = 100, MinHoverDistance = 2;
	[SerializeField] protected Vector3 currentVector;
	[SerializeField] protected bool bMoving;
	[SerializeField] int tries;
	protected NavMeshAgent agent;

	protected virtual void OnEnable () 
	{
		tran = transform;
		currentVector = tran.position;
		agent = GetComponent<NavMeshAgent>();
	}

	public Vector3 RandomVector(Vector3 origin, float range)
	{
		Vector3 rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
		NavMeshPath path = new NavMeshPath();
		agent.CalculatePath(rando,path);
		float dist = Vector3.Distance(rando,tran.position);
		tries = 10;
		while(tries>0 && (dist>MaxHoverDistance || dist<MinHoverDistance) || (path.status == NavMeshPathStatus.PathPartial))
		{
			tries--;
			rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
			agent.CalculatePath(rando,path);
			dist = Vector3.Distance(rando,tran.position);
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

	IEnumerator MovingTo()
	{
		while(bMoving)
		{
			if(agent.remainingDistance<1)
			{
				bMoving = false;
				Debug.Log("I arrived");
				//controller.ArrivedAtTargetLocation(); //Apparently this is causing a huge buffer oveload
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
}
