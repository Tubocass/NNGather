using UnityEngine;
using System.Collections;

public class FarmerController : MonoBehaviour 
{
	public int TeamID;
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
//	public Vector3 Anchor{ 
//		get{
//			if(activeFlag)
//				return farmFlag;
//			else return momTran;
//			}
//	}
	[SerializeField] Transform mouth;
	[SerializeField] float orbit = 25;
	[SerializeField] Vector3 nose;
	//private Transform momTran, farmFlag;
	NavMove navMove;
	bool activeFlag, bInDanger; //bTargeting, bHolding, bReturning, bWandering
	Vector3 foodLoc;
	FoodObject carriedFood, targetedFood;
	MoMController myMoM;
//	private enum State{Wander, Pursue, Return, Idle};
//	State state;

	void OnEnable()
	{
		navMove = GetComponent<NavMove>();
		UnityEventManager.StartListeningInt("PlaceFarmFlag", UpdateFlagLocation);
		UnityEventManager.StartListeningInt("TargetUnavailable", TargetLost);
	}
	void OnDisable()
	{
		UnityEventManager.StopListeningInt("PlaceFarmFlag", UpdateFlagLocation);
		UnityEventManager.StopListeningInt("TargetUnavailable", TargetLost);
		StopCoroutine(Idle());
	}

	public void setMoM(MoMController mom)
	{
		myMoM = mom;
		TeamID = myMoM.TeamID;
		//farmFlag = flag;
		//momTran = mom;
		//activeFlag = farmFlag.gameObject.activeSelf;
		StartCoroutine(Idle());
	}

	void UpdateFlagLocation(int team)
	{
//		if(farmFlag.gameObject.activeSelf)
//		{
//			activeFlag = true;
//		}else activeFlag = false;
		if(TeamID == team && CanTargetFood() && Vector3.Distance(transform.position, myMoM.FoodAnchor)>orbit)
		{
			navMove.MoveTo(myMoM.FoodAnchor);
		}
	}
	void ReturnToHome()
	{
		navMove.MoveTo(myMoM.Location);
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
	void TargetLost(int id)
	{
		if(IsTargetingFood() && id == targetedFood.Id)
		{
			targetedFood = null;
			MoveRandomly();
		}
	}
	void MoveRandomly()
	{
		Vector3 rVector = navMove.RandomVector(myMoM.FoodAnchor, orbit);
		navMove.MoveTo(rVector);
	}
	IEnumerator Idle()
	{
		while(true)
		{
			if(!navMove.BMoving)
			{
				ArrivedAtTargetLocation();
			}
			yield return new WaitForSeconds(1);
		}
	}
	public void ArrivedAtTargetLocation()
	{
		if(IsCarryingFood() && Vector3.Distance(myMoM.Location,transform.position)>1)
		{
			ReturnToHome();
		}
		if(IsTargetingFood() && Vector3.Distance(targetedFood.Location,transform.position)>1)
		{
			navMove.MoveTo(targetedFood.Location);
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
	public void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Food")
		{
			FoodObject ot = other.gameObject.GetComponent<FoodObject>();
			if(ot!=null && ot.CanBeTargetted && CanTargetFood())
			{
				targetedFood = ot;
				navMove.MoveTo(targetedFood.Location);
			}
		}
	}
	public void OnTriggerStay(Collider other)
	{
		if(other.tag == "Food" && CanTargetFood())
		{
			FoodObject ot = other.gameObject.GetComponent<FoodObject>();
			if(ot!=null && ot.CanBeTargetted)
			{
				targetedFood = ot;
				navMove.MoveTo(targetedFood.Location);
			}
		}
	}
	public void OnCollisionEnter(Collision bang)
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

//		if(bang.collider.tag == "Farmer")
//		{
//			return;
//		}
	}
//	public void OnCollisionStay(Collision bang)
//	{
//		if(bang.collider.tag == "Farmer")
//		{
//			return;
//		}
//	}
//	public void OnCollisionExit(Collision bang)
//	{
//		if(bang.collider.tag == "Farmer")
//		{
//			return;
//		}
//	}
}
