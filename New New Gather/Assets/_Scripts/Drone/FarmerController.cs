using UnityEngine;
using System.Collections;

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

	protected override void OnEnable()
	{
		base.OnEnable();
		UnityEventManager.StartListening("PlaceFarmFlag", UpdateFlagLocation);
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		UnityEventManager.StopListening("PlaceFarmFlag", UpdateFlagLocation);
	}

//	public void setMoM(MoMController mom, Color tc)
//	{
//		myMoM = mom;
//		TeamID = myMoM.TeamID;
//		GetComponentInChildren<MeshRenderer>().materials[1].color = tc;
//		StartCoroutine(Idle());
//	}

	protected override void UpdateFlagLocation(int team)
	{
		if(TeamID == team && CanTargetFood() && Vector3.Distance(transform.position, myMoM.FoodAnchor)>orbit)
		{
			MoveTo(myMoM.FoodAnchor);
		}
	}

//	void ReturnToHome()
//	{
//		navMove.MoveTo(myMoM.Location);
//	}

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

	protected override void TargetLost(int id)
	{
		if(IsTargetingFood() && id == targetedFood.Id)
		{
			targetedFood = null;
			MoveRandomly();
		}
	}

	protected override void MoveRandomly()
	{
		Vector3 rVector = RandomVector(myMoM.FoodAnchor, orbit);
		MoveTo(rVector);
	}

//	IEnumerator Idle()
//	{
//		while(true)
//		{
//			if(!navMove.BMoving)
//			{
//				ArrivedAtTargetLocation();
//			}
//			yield return new WaitForSeconds(1);
//		}
//	}

	protected override void ArrivedAtTargetLocation()
	{
		if(IsCarryingFood() && Vector3.Distance(myMoM.Location,transform.position)>1)
		{
			ReturnToHome();
		}
		if(IsTargetingFood() && Vector3.Distance(targetedFood.Location,transform.position)>1)
		{
			MoveTo(targetedFood.Location);
		}
		if(CanTargetFood())
		{
			MoveRandomly();
		}
//		if(isFoodInSight())
//		{
//			//move to food
//		}

	}

	public override void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Food" && CanTargetFood())
		{
			FoodObject ot = other.gameObject.GetComponent<FoodObject>();
			if(ot!=null && ot.CanBeTargetted)
			{
				targetedFood = ot;
				MoveTo(targetedFood.Location);
			}
		}
	}

	public override void OnTriggerStay(Collider other)
	{
		if(other.tag == "Food" && CanTargetFood())
		{
			FoodObject ot = other.gameObject.GetComponent<FoodObject>();
			if(ot!=null && ot.CanBeTargetted)
			{
				targetedFood = ot;
				MoveTo(targetedFood.Location);
			}
		}
	}

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
					//UnityEventManager.TriggerEventInt("TargetUnavailable",ot.Id);
				}
			}
		}
		if(bang.collider.tag == "MoM" && IsCarryingFood())
		{
			bang.gameObject.GetComponent<MoMController>().AddFoodLocation(foodLoc);
			carriedFood.Destroy();
			carriedFood = null;

		}
	}
}
