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
	FoodObject targetedFood, carriedFood;

	protected virtual void OnEnable () 
	{
		maxDistanceSqrd = MaxHoverDistance*MaxHoverDistance;
		minDistanceSqrd = MinHoverDistance*MinHoverDistance;
		tran = transform;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		currentVector = tran.position;
		//StartCoroutine(Idle());
	}

	public Vector3 RandomPath(Vector3 origin, float range)
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
		return rando;
	}

	[Server]
	public void MoveTo(Vector3 location)
	{
		if(!location.Equals(Vector3.zero))
		{
//			currentVector = location; 
//			agent.SetDestination(currentVector);
			NavMeshPath path = new NavMeshPath();
			agent.CalculatePath(location, path);
			agent.SetPath(path);
			RpcMoveTo(location);
		}
	}
	[ClientRpc]
	public void RpcMoveTo(Vector3 loc)
	{
		currentVector = loc; 
		agent.SetDestination(currentVector);
	}
//	[ClientRpc]
//	public void RpcMoveTo(Vector3[] PathArray)
//	{
//		StopCoroutine("MovingTo");
//		bMoving = true;
//		points = PathArray.Length;
//		if(points>0)
//		{
//			currntPoint = 0;
//			Path = PathArray;
//			currentVector = Path[currntPoint];
//			agent.SetDestination(currentVector);
//			StartCoroutine("MovingTo");
//		}
//	}
//
//	protected virtual IEnumerator MovingTo()
//	{
//		while(bMoving)
//		{
//			if(agent.remainingDistance<1)
//			{
//				if(currntPoint<points-1)
//				{
//					currntPoint +=1;
//					currentVector = Path[currntPoint];
//					agent.SetDestination(currentVector);
//				}else bMoving = false;
//				//Debug.Log("I arrived");
//			}
//			yield return new WaitForSeconds(0.5f);
//		}
//	}
//
//	protected virtual IEnumerator Idle()
//	{
//		while(true)
//		{
//			if(!bMoving)
//			{
//				ArrivedAtTargetLocation();
//			}
//			yield return new WaitForSeconds(1);
//		}
//	}
	void Update()
	{
		if(!isServer)
		return;

		if(agent.remainingDistance<1)
		{
			ArrivedAtTargetLocation();
		}
	}
	protected virtual void ArrivedAtTargetLocation()
	{
		if(isServer)
		{
			targetedFood = TargetNearest();
			if(targetedFood!=null)
			{
				MoveTo(targetedFood.Location);
			}
			else MoveRandomly();
		}
	}

	[Server]
	protected void MoveRandomly()//Vector3[] PathArray
	{
		Vector3 rVector = RandomPath(Vector3.zero, 25);
		MoveTo(rVector);
	}

	FoodObject TargetNearest()
	{
		float nearestFoodDist, newDist;
		FoodObject food = null;

		//RaycastHit[] hits = Physics.SphereCastAll(Location,sightRange,tran.forward,1,mask, QueryTriggerInteraction.Ignore);
		Collider[] cols = Physics.OverlapSphere(tran.position,20,1<<LayerMask.NameToLayer("Food"));
		if(cols.Length>0)
		{
			nearestFoodDist = (cols[0].transform.position-transform.position).sqrMagnitude;
			foreach(Collider f in cols)
			{
				if(f.CompareTag("Food"))
				{
					newDist = (f.transform.position-transform.position).sqrMagnitude;
					FoodObject ot = f.GetComponent<FoodObject>();
					if(ot.CanBeTargetted && newDist <= nearestFoodDist)
					{
						nearestFoodDist = newDist;
						food = ot;
					}
				}
			}
		}

		//foods = myMoM.Foods.FindAll(e=> e.CanBeTargetted && (e.Location-Location).sqrMagnitude<sqrDist);
//
//		if(foods.Count>0)
//		{
//			nearestFoodDist = (foods[0].Location-Location).sqrMagnitude; //Vector3.Distance(Location,enemies[0].Location);
//			foreach(FoodObject f in foods)
//			{
//				newDist = (f.Location-Location).sqrMagnitude;//Vector3.Distance(Location,unit.Location);
//				if(newDist <= nearestFoodDist)
//				{
//					nearestFoodDist = newDist;
//					food = f;
//				}
//			}
//		}
		return food;
	}

	bool IsCarryingFood()
	{	
		FoodObject fo = GetComponentInChildren<FoodObject>();
		if(fo !=null)
		{
			if(carriedFood==null)
			{
				carriedFood = fo;
			}
			if(fo.Id != carriedFood.Id)
			{
				carriedFood.Destroy();
				carriedFood = fo;
			}
			return true;
		}else return false;
	}

	bool IsTargetingFood()
	{
		if(targetedFood!=null && targetedFood.gameObject.activeSelf)
		return true;
		else return false;
	}

	bool CanTargetFood()
	{
		if(!IsTargetingFood() && !IsCarryingFood())
		return true;
		else return false;
	}

	public void OnCollisionEnter(Collision bang)
	{
		if(!isServer)
		return;

		if(bang.collider.CompareTag("Food"))
		{
			FoodObject ot = bang.gameObject.GetComponent<FoodObject>();
			if(ot!=null && ot.CanBeTargetted)
			{
				if(IsTargetingFood() || CanTargetFood())
				{
//					if(IsTargetingFood()&& ot.Id != targetedFood.Id)
//					{
//						return;
//					}
					targetedFood = null;
					carriedFood = ot;
					ot.Attach(this.gameObject,new Vector3(0,0,1));
					//foodLoc = ot.Location;
					//StartCoroutine(ReturnToHome());
				}
			}
		}
//		if(IsCarryingFood() && bang.collider.tag == "MoM")
//		{
//			MoMController bangMoM = bang.gameObject.GetComponent<MoMController>();
//			if(bangMoM.unitID == myMoM.unitID)
//			{
//				bangMoM.AddFoodLocation(foodLoc);
//				carriedFood.Destroy();
//				carriedFood = null;
//				bReturning = false;
//			}
//		}
	}
	public void OnTriggerEnter(Collider other)
	{}

}
