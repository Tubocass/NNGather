using UnityEngine;
using System.Collections;

public class NavMove : MonoBehaviour 
{
	//Functionality moved to Unit_Base
//	public bool BMoving{get{return bMoving;}}
//	[SerializeField] Vector3 rVector;
//	[SerializeField] float MaxHoverDistance = 100, MinHoverDistance = 2;
//	[SerializeField] int tries;
//	[SerializeField] bool bMoving;
//	Transform tran;
//	NavMeshAgent agent;
//
//
//	void OnEnable () 
//	{
//		tran = transform;
//		rVector = tran.position;
//		agent = GetComponent<NavMeshAgent>();
////		bWandering = true;
////		StartCoroutine("Wander");
//	}
//
////	IEnumerator Wander()
////	{
////		while(bWandering)
////		{
////			if(agent.remainingDistance<1)
////			{
////				rVector = RandomVector(controller.Anchor.position, orbit);
////				agent.SetDestination(rVector);
////			}
////			yield return new WaitForSeconds(1);
////		}
////	}
//	IEnumerator MovingTo()
//	{
//		while(bMoving)
//		{
//			if(agent.remainingDistance<1)
//			{
//				bMoving = false;
//				Debug.Log("I arrived");
//				//controller.ArrivedAtTargetLocation(); //Apparently this is causing a huge buffer oveload
//			}
//			yield return new WaitForSeconds(0.5f);
//		}
//	}
//
//	public Vector3 RandomVector(Vector3 origin, float range)
//	{
//		Vector3 rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
//		NavMeshPath path = new NavMeshPath();
//		agent.CalculatePath(rando,path);
//		float dist = Vector3.Distance(rando,tran.position);
//		tries = 10;
//		while(tries>0 && (dist>MaxHoverDistance || dist<MinHoverDistance) || (path.status == NavMeshPathStatus.PathPartial))
//		{
//			tries--;
//			rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
//			agent.CalculatePath(rando,path);
//			dist = Vector3.Distance(rando,tran.position);
//		}
//		return rando;
//	}
//
//	public void MoveTo(Vector3 location)
//	{
//		//agent.ResetPath();
//		bMoving = true;
//		rVector = location;
//		agent.SetDestination(location);
//		StopCoroutine("MovingTo");
//		StartCoroutine("MovingTo");
//
//	}
}
