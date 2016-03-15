using UnityEngine;
using System.Collections;

public class BWander : IBehaviour 
{
//	public Transform target;
//	[SerializeField] Vector3 rVector;
//	[SerializeField] float MaxDistance = 200, MinDistance = 16, orbit = 25;
//	[SerializeField] int tries = 5;
//	[SerializeField] bool bWandering;
//	Transform tran;
//	NavMeshAgent agent;
//
	public IEnumerator EnterState()
	{
		/*
		while(bWandering)
		{
			if(agent.remainingDistance<1)
			{
				rVector = RandomVector(controller.Anchor.position, orbit);
				agent.SetDestination(rVector);
			}
			yield return new WaitForSeconds(1);
		}
	*/
	yield return null;
	}
	public void Update ()
	{}
	public void ExitState ()
	{}
	public void ToWander ()
	{}
}
