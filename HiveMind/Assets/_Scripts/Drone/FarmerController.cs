using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
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
	LayerMask mask;
	List<FoodObject> foods = new List<FoodObject>();
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

	protected IEnumerator ReturnToHome()
	{
		if(isServer)
		{
			bReturning = true;
			while(bReturning && myMoM!=null)
			{
				MoveTo(myMoM.Location);
				yield return new WaitForSeconds(1f);
			}
		}
		yield return null;
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
				StartCoroutine(ReturnToHome());
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
				else MoveRandomly(myMoM.FoodAnchor,orbit);
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
		if(!IsTargetingFood() && !IsCarryingFood())
		return true;
		else return false;
	}

	FoodObject TargetNearest()
	{
		float nearestFoodDist, newDist;
		FoodObject food = null;
		//foods.Clear();
		//RaycastHit[] hits = Physics.SphereCastAll(Location,sightRange,tran.forward,1,mask, QueryTriggerInteraction.Ignore);

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
		}else
		{
			Collider[] cols = Physics.OverlapSphere(tran.position,sightRange,mask);
			if(cols.Length>0)
			{
				foreach(Collider f in cols)
				{
					if(f.CompareTag("Food"))
					{
						FoodObject ot = f.GetComponent<FoodObject>();
						if(ot!=null && ot.CanBeTargetted && !myMoM.Foods.Contains(ot))
						{
							myMoM.Foods.Add(ot);
						}
					}
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
					ot.Attach(this.gameObject,nose);
//					childTransform.enabled = true;
//					childTransform.target = ot.transform;
					foodLoc = ot.Location;
					StartCoroutine(ReturnToHome());
				}
			}
		}

		if(IsCarryingFood() && bang.collider.tag == "MoM")
		{
			MoMController bangMoM = bang.gameObject.GetComponent<MoMController>();
			if(bangMoM.unitID == myMoM.unitID)
			{
				bangMoM.AddFoodLocation(foodLoc);
//				childTransform.target = null;
//				childTransform.enabled = false;
				carriedFood.Destroy();
				carriedFood = null;
				bReturning = false;
			}
		}
	}
}
