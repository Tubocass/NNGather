using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FarmerController : DroneController 
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
	static List<FoodObject> Foods;
	List<FoodObject> foods;
	bool bReturning;

	protected override void OnEnable()
	{
		base.OnEnable();
		Foods = new List<FoodObject>();
		UnityEventManager.StartListening("PlaceFarmFlag", UpdateFlagLocation);
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		UnityEventManager.StopListening("PlaceFarmFlag", UpdateFlagLocation);
	}

	protected override void UpdateFlagLocation(int team)
	{
		if(teamID == team && CanTargetFood() && Vector3.Distance(transform.position, myMoM.FoodAnchor)>orbit)
		{
			MoveTo(myMoM.FoodAnchor);
		}
	}

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

	protected override void MoveRandomly()
	{
		Vector3 rVector = RandomVector(myMoM.FoodAnchor, orbit);
		MoveTo(rVector);
	}

	protected override IEnumerator MovingTo()
	{
		while(bMoving)
		{
			if(agent.remainingDistance<1)
			{
				bMoving = false;
				//Debug.Log("I arrived");
				//controller.ArrivedAtTargetLocation(); //Apparently this is causing a huge buffer oveload
			}else
			{
				yield return new WaitForSeconds(0.5f);
				if(bReturning) MoveTo(myMoM.Location);
			}
		}
	}

	protected override void TargetLost(int id)
	{
		if(IsTargetingFood() && id == targetedFood.Id)
		{
			targetedFood = null;
			MoveRandomly();
		}
	}

	protected override void ArrivedAtTargetLocation()
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

	bool IsFoodInSight()
	{
		return false;
	}

	FoodObject TargetNearest()
	{
		float nearestFoodDist, newDist;
		FoodObject food = null;

		foods = Foods.FindAll(e=> e.CanBeTargetted && (e.Location-Location).sqrMagnitude<sqrDist);

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

	protected void ReturnToHome()
	{
		bReturning = true;
		MoveTo(myMoM.Location);
	}

	public override void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Food")
		{
			FoodObject ot = other.gameObject.GetComponent<FoodObject>();
			if(ot!=null && !Foods.Contains(ot))
			{
				Foods.Add(ot);
			}
		}
	}

//	public override void OnTriggerStay(Collider other)
//	{
//		
////		if(other.tag == "Food")
////		{
////			if(targetedFood==null)
////			{
////				FoodObject ot = other.gameObject.GetComponent<FoodObject>();
////				if(ot!=null && ot.CanBeTargetted)
////				{
////					targetedFood = ot;
////					MoveTo(targetedFood.Location);
////				}
////			}
////		}
//	}

	public override void OnCollisionEnter(Collision bang)
	{
		if(bang.collider.tag == "Food")
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
					ot.Attach(transform,nose);
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
