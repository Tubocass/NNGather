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

	protected static List<FarmerController> Farmers = new List<FarmerController>();
	protected static List<FighterController> Fighters = new List<FighterController>();
	protected Transform farmFlagTran, fightFlagTran;
	protected bool activeFarmFlag, activeFightFlag;
	[SerializeField] public int farmers, fighters;
	[SerializeField] protected GameObject farmer, soldier, farmFlag, fightFlag;
	[SerializeField] Color TeamColor;
	MeshRenderer currentMesh;
	Queue<Vector3> foodQ = new Queue<Vector3>(10);
	//NavMeshAgent agent;

	protected override void OnEnable()
	{
		base.OnEnable();
		Farmers = new List<FarmerController>();
		Fighters = new List<FighterController>();
		teamID = MoMCount+1;
	}
	protected virtual void OnDisable()
	{
		StopCoroutine(UpdateLocation());
	}

	protected virtual void Start()
	{	
		OnCreated();
		currentMesh = GetComponentInChildren<MeshRenderer>();
		currentMesh.material.color = TeamColor;
		farmFlag = Instantiate(farmFlag) as GameObject; //GameObject.Find("FarmFlag");
		fightFlag = Instantiate(fightFlag) as GameObject;//GameObject.Find("FightFlag");
		farmFlagTran = farmFlag.GetComponent<Transform>();
		fightFlagTran = fightFlag.GetComponent<Transform>();
		StartCoroutine(UpdateLocation());
	}
	public override void OnCreated()
	{
		base.OnCreated();
		MoMCount+=1;
	}

	public virtual void PlaceFarmFlag(Vector3 location)
	{
		farmFlag.SetActive(true);
		farmFlagTran.position = location;
		farmFlag.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEvent("PlaceFarmFlag", teamID);
		activeFarmFlag = true;
	}
	public virtual void RecallFarmFlag()
	{
		farmFlagTran.position = transform.position;
		farmFlag.GetComponent<ParticleSystem>().Stop();
		farmFlag.SetActive(false);
		UnityEventManager.TriggerEvent("PlaceFarmFlag", teamID);
		activeFarmFlag = false;
	}
	public virtual void PlaceFightFlag(Vector3 location)
	{
		fightFlag.SetActive(true);
		fightFlagTran.position = location;
		fightFlag.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEvent("PlaceFightFlag", teamID);
		activeFightFlag = true;
	}
	public virtual void RecallFightFlag()
	{
		fightFlagTran.position = transform.position;
		fightFlag.GetComponent<ParticleSystem>().Stop();
		fightFlag.SetActive(false);
		UnityEventManager.TriggerEvent("PlaceFightFlag", teamID);
		activeFightFlag = false;
	}
	public virtual void CreateFarmer()
	{
		if(FoodAmount>0)
		{
			FoodAmount -= 1;
			farmers++;
			if(Farmers.Count>0)
			{
				FarmerController recycle = Farmers.Find(f=> !f.isActive);
				if(recycle!=null)
				{
					recycle.setMoM(this, TeamColor);
				}else{
					InstantiateFarmer();
				}
			}else {
				InstantiateFarmer();
			}
		}
	}

	public virtual void CreateFighter()
	{
		if(FoodAmount>0)
		{
			FoodAmount -= 1;
			fighters++;
			if(Fighters.Count>0)
			{
				FighterController recycle = Fighters.Find(f=> !f.isActive);
				if(recycle!=null)
				{
					recycle.setMoM(this, TeamColor);
				}else{
					InstantiateFighter();
				}
			}else {
				InstantiateFighter();
			}
		}
	}
	void InstantiateFarmer()
	{
		GameObject spawn = Instantiate(farmer, Location + new Vector3(1,0,1),Quaternion.identity) as GameObject;
		FarmerController fc = spawn.GetComponent<FarmerController>();
		fc.OnCreated();
		fc.setMoM(this, TeamColor);
		Farmers.Add(fc);
	}
	void InstantiateFighter()
	{
		GameObject spawn = Instantiate(soldier,transform.position + new Vector3(1,0,1),Quaternion.identity) as GameObject;
		FighterController fc = spawn.GetComponent<FighterController>();
		fc.OnCreated();
		fc.setMoM(this, TeamColor);
		Fighters.Add(fc);
	}
	public void LoseFarmer(FarmerController unit)
	{
		
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
