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
			else return Location;
			}
	}
	public Vector3 FightAnchor
	{
		get{
			if(activeFightFlag)
				return fightFlagTran.position;
			else return Location;
			}
	}

	protected static List<FarmerController> Farmers = new List<FarmerController>();
	protected static List<FighterController> Fighters = new List<FighterController>();
	protected static List<DaughterController> Daughters = new List<DaughterController>();
	protected Transform farmFlagTran, fightFlagTran;
	protected bool activeFarmFlag, activeFightFlag;
	[SerializeField] public int farmers, fighters, daughters;
	[SerializeField] protected GameObject farmerFab, fighterFab, daughterFab, farmFlagFab, fightFlagFab;
	[SerializeField] protected Color TeamColor;
	Queue<Vector3> foodQ = new Queue<Vector3>(10);

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
		StartCoroutine(UpdateLocation());
	}
	public override void OnCreated()
	{
		base.OnCreated();
		GetComponentInChildren<MeshRenderer>().material.color = TeamColor;
		MoMCount+=1;
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

	public virtual void CreateDaughter()
	{
		if(FoodAmount>0)
		{
			FoodAmount -= 5;
			daughters++;
			if(Farmers.Count>0)
			{
				DaughterController recycle = Daughters.Find(f=> !f.isActive);
				if(recycle!=null)
				{
					recycle.setMoM(this, TeamColor);
				}else{
					InstantiateDaughter();
				}
			}else {
				InstantiateDaughter();
			}
		}
	}

	void InstantiateFarmer()
	{
		GameObject spawn = Instantiate(farmerFab, Location + new Vector3(1,0,1),Quaternion.identity) as GameObject;
		FarmerController fc = spawn.GetComponent<FarmerController>();
		fc.OnCreated();
		fc.setMoM(this, TeamColor);
		Farmers.Add(fc);
	}
	void InstantiateFighter()
	{
		GameObject spawn = Instantiate(fighterFab,transform.position + new Vector3(1,0,1),Quaternion.identity) as GameObject;
		FighterController fc = spawn.GetComponent<FighterController>();
		fc.OnCreated();
		fc.setMoM(this, TeamColor);
		Fighters.Add(fc);
	}
	void InstantiateDaughter()
	{
		GameObject spawn = Instantiate(daughterFab, Location + new Vector3(1,1,1),Quaternion.identity) as GameObject;
		DaughterController dc = spawn.GetComponent<DaughterController>();
		dc.setMoM(this, TeamColor);
		dc.OnCreated();
		Daughters.Add(dc);
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
