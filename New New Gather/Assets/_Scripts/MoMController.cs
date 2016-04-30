using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MoMController : Unit_Base 
{
	public static int MoMCount;
	public int FoodAmount;
	public Vector3 FoodAnchor
	{
		get{
			if(activeFarmFlag)
				return farmFlagTran.position;
			else return transform.position;
			}
	}
	public Vector3 FightAnchor
	{
		get{
			if(activeFightFlag)
				return fightFlagTran.position;
			else return transform.position;
			}
	}
	//public Vector3 Location{get{return transform.position;}}
	protected Transform farmFlagTran, fightFlagTran;
	protected bool activeFarmFlag, activeFightFlag;
	protected static List<FarmerController> Farmers;
	protected static List<FighterController> Fighters;
	[SerializeField] protected GameObject farmer, soldier, farmFlag, fightFlag;
	[SerializeField] Color TeamColor;
	MeshRenderer currentMesh;
	Queue<Vector3> foodQ = new Queue<Vector3>(10);
	//NavMeshAgent agent;

	protected virtual void OnEnable()
	{
		base.OnEnable();
		Farmers = new List<FarmerController>();
		Fighters = new List<FighterController>();
		TeamID = MoMCount+1;
	}
	protected virtual void OnDisable()
	{
		StopCoroutine(UpdateLocation());
	}

	protected virtual void Start()
	{	
		currentMesh = GetComponentInChildren<MeshRenderer>();
		currentMesh.material.color = TeamColor;
		farmFlag = Instantiate(farmFlag) as GameObject; //GameObject.Find("FarmFlag");
		fightFlag = Instantiate(fightFlag) as GameObject;//GameObject.Find("FightFlag");
		farmFlagTran = farmFlag.GetComponent<Transform>();
		fightFlagTran = fightFlag.GetComponent<Transform>();
		StartCoroutine(UpdateLocation());
	}

	public virtual void PlaceFarmFlag(Vector3 location)
	{
		farmFlag.SetActive(true);
		farmFlagTran.position = location;
		farmFlag.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEvent("PlaceFarmFlag", TeamID);
		activeFarmFlag = true;
	}
	public virtual void RecallFarmFlag()
	{
		farmFlagTran.position = transform.position;
		farmFlag.GetComponent<ParticleSystem>().Stop();
		farmFlag.SetActive(false);
		UnityEventManager.TriggerEvent("PlaceFarmFlag", TeamID);
		activeFarmFlag = false;
	}
	public virtual void PlaceFightFlag(Vector3 location)
	{
		fightFlag.SetActive(true);
		fightFlagTran.position = location;
		fightFlag.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEvent("PlaceFightFlag", TeamID);
		activeFightFlag = true;
	}
	public virtual void RecallFightFlag()
	{
		fightFlagTran.position = transform.position;
		fightFlag.GetComponent<ParticleSystem>().Stop();
		fightFlag.SetActive(false);
		UnityEventManager.TriggerEvent("PlaceFightFlag", TeamID);
		activeFightFlag = false;
	}
	public virtual void CreateFarmer()
	{
		if(FoodAmount>0)
		{
			FoodAmount -= 1;
			if(Farmers.Count>0)
			{
				FarmerController recycle = Farmers.Find(f=> !f.isActive);
				if(recycle!=null)
				{
					recycle.setMoM(this, TeamColor);
				}else{
					GameObject spawn = Instantiate(farmer,transform.position + new Vector3(1,0,1),Quaternion.identity) as GameObject;
					FarmerController fc = spawn.GetComponent<FarmerController>();
					fc.setMoM(this, TeamColor);
					Farmers.Add(fc);
				}
			}else {
				GameObject spawn = Instantiate(farmer,transform.position + new Vector3(1,0,1),Quaternion.identity) as GameObject;
				FarmerController fc = spawn.GetComponent<FarmerController>();
				fc.setMoM(this, TeamColor);
				Farmers.Add(fc);
			}
		}
	}
	public void LoseFarmer(FarmerController unit)
	{
		
	}

	public virtual void CreateFighter()
	{
		if(FoodAmount>0)
		{
			FoodAmount -= 1;
			GameObject spawn = Instantiate(soldier,transform.position + new Vector3(1,0,1),Quaternion.identity) as GameObject;
			spawn.GetComponent<FighterController>().setMoM(this, TeamColor);
		}
	}

	public virtual void AddFoodLocation(Vector3 loc)
	{
		if(foodQ.Count>9)
		{
			foodQ.Dequeue();
			foodQ.Enqueue(loc);
			Debug.Log("newness");
		}else {foodQ.Enqueue(loc);Debug.Log("newness");}

		FoodAmount++;
	}

	IEnumerator UpdateLocation()
	{
		while(true)
		{
			if(foodQ.Count>0)
			MoveToCenter();
			yield return new WaitForSeconds(3);
		}
	}

	void MoveToCenter()
	{
		Debug.Log("Updating");
		Vector3 newLoc;
		float xx = 0, zz = 0;
		int size = 1;
		foreach(Vector3 v in foodQ)
		{
			if(v != Vector3.zero)
			{
				size++;
				xx += v.x;
				zz += v.z;
			}
		}
		newLoc = new Vector3(xx /size, 1f ,zz /size);
		if(Vector3.Distance( transform.position, newLoc)>1)
		{
			Debug.Log("I'm gonna move");
			agent.SetDestination(newLoc);
		}
	}
}
