using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class NetworkTestFarmer : NetworkBehaviour
{
	public bool isActive{get{return gameObject.activeSelf;}set{gameObject.SetActive(value); if(value==false)OnDisable();}}
	public Vector3 Location{get{return transform.position;}}
	[SyncVar(hook = "OnChangeColor")] public Color TeamColor;
	[SerializeField] protected float MaxHoverDistance = 20, MinHoverDistance = 1;
	[SerializeField] protected Vector3 currentVector;
	[SerializeField] int tries;
	[SerializeField] Vector3[] pathArray;
	protected Transform tran;
	protected UnityEngine.AI.NavMeshAgent agent;
	float maxDistanceSqrd, minDistanceSqrd;
	FoodObject targetedFood, carriedFood;
	public Interact myMoM;
	bool bReturning, hasChanged;
	Vector3 foodLoc;
	Material TeamColorMat;
	int teamID, currentPoint;
	[SyncVar] bool bMoving = false;

	protected virtual void OnEnable () 
	{
		maxDistanceSqrd = MaxHoverDistance*MaxHoverDistance;
		minDistanceSqrd = MinHoverDistance*MinHoverDistance;
		tran = transform;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		TeamColorMat = GetComponentInChildren<MeshRenderer>().material;
		currentVector = tran.position;
	}
	protected void OnDisable()
	{}
	protected void OnChangeColor(Color newColor)
	{
		TeamColor = newColor;
		TeamColorMat.color = newColor;
		hasChanged = true;
	}
	public override void OnStartClient()
	{
		if(hasChanged)
		TeamColorMat.color = TeamColor;
	}

	public virtual void SetMoM(GameObject mom, Color tc)
	{
		isActive = true;
		myMoM = mom.GetComponent<Interact>();
		teamID = myMoM.teamID;
		GetComponentInChildren<MeshRenderer>().materials[1].color = tc;
	}
	public Vector3 RandomPath(Vector3 origin, float range)
	{
		Vector3 rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
		//UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();
		//agent.CalculatePath(rando,path);
		float dist = (rando-tran.position).sqrMagnitude;
		tries = 10;
		while(tries>0 && (dist>maxDistanceSqrd|| dist<minDistanceSqrd))// || (path.status == UnityEngine.AI.NavMeshPathStatus.PathPartial))
		{
			tries--;
			rando = new Vector3(Random.Range(-range,range)+origin.x, origin.y,Random.Range(-range,range)+origin.z);
			//agent.CalculatePath(rando,path);
			dist = (rando-tran.position).sqrMagnitude;
		}
		return rando;
	}

	[Server]
	public void MoveTo(Vector3 location)
	{
		if(!location.Equals(Vector3.zero))
		{
			NavMeshPath path = new NavMeshPath();
			agent.CalculatePath(location, path);
			agent.SetPath(path);
			pathArray = path.corners;
			if(path.corners.Length>2)
			{
				bMoving = true;
				currentPoint = 1;
				currentVector = path.corners[currentPoint];
			}else{ currentVector = path.corners[1];}
			RpcMoveTo(currentVector);
		}
	}
	[ClientRpc]
	public void RpcMoveTo(Vector3 loc)
	{
		currentVector = loc; 
		agent.SetDestination(currentVector);
	}

	void Update()
	{
		if(isServer)
		{
			if(agent.remainingDistance<1)
			{
				ArrivedAtTargetLocation();
				bMoving = false;
			}else{
				if(bMoving&& currentPoint<agent.path.corners.Length && Vector3.Distance(tran.position,currentVector)<1f)
				{
					currentPoint++;
					currentVector = agent.path.corners[currentPoint];
					RpcMoveTo(currentVector);
				}
			}
		}
	}
	protected virtual void ArrivedAtTargetLocation()
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
				else MoveRandomly();
			}
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
					if(IsTargetingFood()&& ot.Id != targetedFood.Id)
					{
						return;
					}
					targetedFood = null;
					carriedFood = ot;
					ot.Attach(this.gameObject,new Vector3(0,0,1));
					foodLoc = ot.Location;
					StartCoroutine(ReturnToHome());
				}
			}
		}
		if(IsCarryingFood() && bang.collider.tag == "MoM")
		{
			Interact bangMoM = bang.gameObject.GetComponent<Interact>();
			if(bangMoM.unitID == myMoM.unitID)
			{
				//bangMoM.AddFoodLocation(foodLoc);
				carriedFood.Destroy();
				carriedFood = null;
				bReturning = false;
			}
		}
	}
	public void OnTriggerEnter(Collider other)
	{}
	protected IEnumerator ReturnToHome()
	{
		if(isServer)
		{
			bReturning = true;
			while(bReturning && myMoM!=null)
			{
				MoveTo(myMoM.transform.position);
				yield return new WaitForSeconds(5f);
			}
		}
		yield return null;
	}

}
