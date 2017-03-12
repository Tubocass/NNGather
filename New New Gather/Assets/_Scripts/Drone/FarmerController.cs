using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class FarmerController : DroneController, IAttachable 
{
	/*public IBehaviour BState 
	{
		get{ return behaviourState; }
		set 
		{
			if (behaviourState != null)
			{
				behaviourState.ExitState ();
			}
			behaviourState = value;
			behaviourState.EnterState ();
		}
	}
	private IBehaviour behaviourState;
	public BWander WanderState;
	*/
	Vector3 foodLoc;
	FoodObject carriedFood, targetedFood;
	LayerMask mask;
	List<FoodObject> foods;
	bool bReturning;

	protected override void OnEnable()
	{
		base.OnEnable();
		mask = 1<<LayerMask.NameToLayer("Food");
		UnityEventManager.StartListeningInt("PlaceFarmFlag", UpdateFlagLocation);
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		UnityEventManager.StopListeningInt("PlaceFarmFlag", UpdateFlagLocation);
	}

	protected override void UpdateFlagLocation(int team)
	{
		if(isServer)
		{
			if(myMoM.unitID == team && !IsCarryingFood() && Vector3.Distance(transform.position, myMoM.FoodAnchor)>orbit)
			{
				targetedFood = null;
				MoveTo(myMoM.FoodAnchor);
			}
		}
	}
//	public override void setMoM(MoMController mom, Color tc)
//	{
//		base.setMoM(mom, tc);
//		//StartCoroutine(LookForFood());
//	}

	protected override void Death()
	{
		base.Death();
		myMoM.farmers-=1;
		if(IsCarryingFood())
		{
			carriedFood.Detach();
			carriedFood = null;
		}
	}

//	protected override void MoveRandomly()
//	{
//		Vector3 rVector = RandomVector(myMoM.FoodAnchor, orbit);
//		MoveTo(rVector);
//	}

//	protected override void MoveRandomly()
//	{
//		NavMeshPath rVector = RandomPath(myMoM.FoodAnchor, orbit);
//		if(rVector.status!=NavMeshPathStatus.PathPartial)
//		{
//			agent.SetPath(rVector);
//			agent.Resume();
//		}
//	}

	protected override IEnumerator MovingTo()
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
			}
			//if(bReturning&&Vector3.Distance(myMoM.Location,currentVector)>1) ReturnToHome();
			yield return new WaitForSeconds(0.5f);
		}
	}

	protected void ReturnToHome()
	{
		if(isServer)
		{
			bReturning = true;
			MoveTo(myMoM.Location);
		}
	}

	protected override void TargetLost(int id)
	{
		if(IsTargetingFood() && id == targetedFood.Id)
		{
			targetedFood = null;
			ArrivedAtTargetLocation();
		}
	}

	protected override void ArrivedAtTargetLocation()
	{
		if(isServer && myMoM!=null)
		{
			if(IsCarryingFood() && Vector3.Distance(myMoM.Location, Location)>1)
			{
				ReturnToHome();
			}
			if(IsTargetingFood() && Vector3.Distance(targetedFood.Location, Location)>1)
			{
				MoveTo(targetedFood.Location);
			}
			if(CanTargetFood())
			{
				targetedFood = TargetNearest();
				if(targetedFood!=null)
				{
					MoveTo(targetedFood.Location);
				}
				else MoveRandomly();
			}
		}
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
		if(!IsTargetingFood() && !IsCarryingFood() && !bInDanger)
		return true;
		else return false;
	}

	FoodObject TargetNearest()
	{
		float nearestFoodDist, newDist;
		FoodObject food = null;

		RaycastHit[] hits = Physics.SphereCastAll(Location,sightRange,tran.forward,1,mask, QueryTriggerInteraction.Ignore);
		if(hits.Length>0)
		{
			foreach(RaycastHit f in hits)
			{
				if(f.collider.CompareTag("Food"))
				{
					FoodObject ot = f.collider.GetComponent<FoodObject>();
					if(ot!=null && !myMoM.Foods.Contains(ot))
					{
						myMoM.Foods.Add(ot);
					}
				}
			}
		}

		foods = myMoM.Foods.FindAll(e=> e.CanBeTargetted && (e.Location-Location).sqrMagnitude<sqrDist);

		if(foods.Count>0)
		{
			nearestFoodDist = (foods[0].Location-Location).sqrMagnitude; //Vector3.Distance(Location,enemies[0].Location);
			foreach(FoodObject f in foods)
			{
				newDist = (f.Location-Location).sqrMagnitude;//Vector3.Distance(Location,unit.Location);
				if(newDist <= nearestFoodDist)
				{
					nearestFoodDist = newDist;
					food = f;
				}
			}
		}
		return food;
	}

	public override void OnCollisionEnter(Collision bang)
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
					if(IsTargetingFood()&& ot.Id != targetedFood.Id)
					{
						return;
					}
					targetedFood = null;
					carriedFood = ot;
					ot.RpcAttach(this.gameObject,nose);
					foodLoc = ot.Location;
					ReturnToHome();
				}
			}
		}

		if(IsCarryingFood() && bang.collider.tag == "MoM")
		{
			MoMController bangMoM = bang.gameObject.GetComponent<MoMController>();
			if(bangMoM.unitID == myMoM.unitID)
			{
				bangMoM.AddFoodLocation(foodLoc);
				carriedFood.Destroy();
				carriedFood = null;
				bReturning = false;
			}
		}
	}
}
