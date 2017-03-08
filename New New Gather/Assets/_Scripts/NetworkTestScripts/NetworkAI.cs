using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class NetworkAI : NetworkBehaviour
{
	[SerializeField] protected float MaxHoverDistance = 20, MinHoverDistance = 1;
	[SerializeField] protected Vector3 currentVector;
	[SerializeField] protected bool bMoving;
	[SerializeField] int tries;
	[SerializeField] Vector3[] Path;
	[SerializeField] int points, currntPoint;
	protected Transform tran;
	protected UnityEngine.AI.NavMeshAgent agent;
	protected bool bDay;
	float maxDistanceSqrd, minDistanceSqrd;

	protected virtual void OnEnable () 
	{
		maxDistanceSqrd = MaxHoverDistance*MaxHoverDistance;
		minDistanceSqrd = MinHoverDistance*MinHoverDistance;
		tran = transform;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		currentVector = tran.position;
		StartCoroutine(Idle());
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

	[ClientRpc]
	public void RpcMoveTo(Vector3[] PathArray)
	{
		StopCoroutine("MovingTo");
		bMoving = true;
		points = PathArray.Length;
		if(points>0)
		{
			currntPoint = 0;
			Path = PathArray;
			currentVector = Path[currntPoint];
			agent.SetDestination(currentVector);
			StartCoroutine("MovingTo");
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

	protected virtual IEnumerator Idle()
	{
		while(true)
		{
			if(!bMoving)
			{
				ArrivedAtTargetLocation();
			}
			yield return new WaitForSeconds(1);
		}
	}

	protected virtual void ArrivedAtTargetLocation()
	{
		if(isServer)
		MoveRandomly();
	}

	[Server]
	protected void MoveRandomly()//Vector3[] PathArray
	{
		NavMeshPath rVector = RandomPath(Vector3.zero, 25);
		RpcMoveTo(rVector.corners);
	}
}
