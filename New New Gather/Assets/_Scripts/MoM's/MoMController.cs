using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MoMController : Unit_Base 
{
	public static int MoMCount;
	public int FoodAmount{
		get
		{
			return foodAmount;
		}
		set
		{
			foodAmount+=value; 
			if(this.GetType()==typeof(MainMomController))
			{
				UnityEventManager.TriggerEvent("UpdateFood", foodAmount);
			}
		}
	}
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
	public List<FoodObject> Foods;
	public Color TeamColor;
	public int farmers, fighters, daughters;//counters
	protected static List<FarmerController> Farmers = new List<FarmerController>();//object pool
	protected static List<FighterController> Fighters = new List<FighterController>();//object pool
	protected static List<DaughterController> Daughters = new List<DaughterController>();//object pool
	protected Transform farmFlagTran, fightFlagTran;
	protected bool activeFarmFlag, activeFightFlag;
	[SerializeField] protected GameObject farmerFab, fighterFab, daughterFab, eMoMFAb, mMoMFab,  farmFlagFab, fightFlagFab;
	[SerializeField] protected int foodAmount;
	[SerializeField] int farmerCost=1, fighterCost=2, daughterCost=8, startFood = 5;
	[SerializeField] int qCount=0;
	Queue<Vector3> foodQ = new Queue<Vector3>(10);

	protected override void OnEnable()
	{
		base.OnEnable();
		Foods = new List<FoodObject>();
		GetComponentInChildren<MeshRenderer>().material.color = TeamColor;
		MoMCount+=1;
		daughters = 0;//I don't know why this has to be reset
		foodAmount = startFood;
		teamID = MoMCount+1;
	}
//	protected virtual void OnDisable()
//	{
//		StopCoroutine(UpdateLocation());
//	}

	protected virtual void Start()
	{	
		StartCoroutine(UpdateLocation());
		StartCoroutine(Hunger());
	}
	protected override void Death ()
	{
		base.Death ();
		newQueen();
	}
	public override void TakeDamage(float damage)
	{
		Health = -damage/2;
	}
	protected virtual IEnumerator Hunger()
	{
		while (true)
		{
			yield return new WaitForSeconds(10);
			if(FoodAmount>=1)
			{
				FoodAmount = -1;
			}else Health = -1;
		}
	}

	public virtual void CreateFarmer()
	{
		if(FoodAmount>=farmerCost)
		{
			FoodAmount = -farmerCost;
			farmers++;
			if(Farmers.Count>0)
			{
				FarmerController recycle = Farmers.Find(f=> !f.isActive);
				if(recycle!=null)
				{
					recycle.setMoM(this, TeamColor);
					recycle.transform.position = Location+new Vector3(1,0,1);
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
		if(FoodAmount>=fighterCost)
		{
			FoodAmount = -fighterCost;
			fighters++;
			if(Fighters.Count>0)
			{
				FighterController recycle = Fighters.Find(f=> !f.isActive);
				if(recycle!=null)
				{
					recycle.setMoM(this, TeamColor);
					recycle.transform.position = Location+new Vector3(1,0,1);
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
		if(FoodAmount>=daughterCost)
		{
			FoodAmount = -daughterCost;
			daughters++;
			if(Daughters.Count>0)
			{
				DaughterController recycle = Daughters.Find(f=> !f.isActive);
				if(recycle!=null)
				{
					recycle.setMoM(this, TeamColor);
					recycle.transform.position = Location+new Vector3(1,0,1);
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
		fc.setMoM(this, TeamColor);
		Farmers.Add(fc);
	}
	void InstantiateFighter()
	{
		GameObject spawn = Instantiate(fighterFab,transform.position + new Vector3(1,0,1),Quaternion.identity) as GameObject;
		FighterController fc = spawn.GetComponent<FighterController>();
		fc.setMoM(this, TeamColor);
		Fighters.Add(fc);
	}
	void InstantiateDaughter()
	{
		GameObject spawn = Instantiate(daughterFab, Location + new Vector3(1,1,1),Quaternion.identity) as GameObject;
		DaughterController dc = spawn.GetComponent<DaughterController>();
		dc.setMoM(this, TeamColor);
		Daughters.Add(dc);
	}
	public virtual void CedeDrones(MoMController newMoM)
	{
		List<FarmerController> farmTransfers = Farmers.FindAll(f=> f.isActive && f.myMoM==this);
		List<FighterController> fightTransfers = Fighters.FindAll(f=> f.isActive && f.myMoM==this);
		foreach(FarmerController f in farmTransfers)
			{
				f.setMoM(newMoM);
				newMoM.farmers++;
			}
			foreach(FighterController f in fightTransfers)
			{
				f.setMoM(newMoM);
				newMoM.fighters++;
			}
	}
	protected void KillDrones()
	{
		List<FarmerController> farmTransfers = Farmers.FindAll(f=> f.isActive && f.myMoM==this);
		List<FighterController> fightTransfers = Fighters.FindAll(f=> f.isActive && f.myMoM==this);
		foreach(FarmerController f in farmTransfers)
			{
				f.Health = -20;
			}
			foreach(FighterController f in fightTransfers)
			{
				f.Health = -20;
			}
	}


	public void SetupQueen(MoMController oldMoM)//Gets called by new MoM
	{
		//Health = startHealth;
		teamID = oldMoM.teamID;
		TeamColor = oldMoM.TeamColor;
		GetComponentInChildren<MeshRenderer>().material.color = TeamColor;
		oldMoM.CedeDrones(this);
	}
	protected virtual void newQueen()
	{
		List<DaughterController> princesses = new List<DaughterController>();
		if(Daughters.Count>0)
		{
			princesses = Daughters.FindAll(f=> f.isActive && f.myMoM==this);
		}

		if(princesses.Count>0)
		{
			GameObject spawn;
			//give first princess all drones
			CedeDrones(princesses[0]);

			for(int p = 0; p<princesses.Count; p++)
			{
				if(this.GetType()== typeof(MainMomController) && p==0)
				{
					spawn = Instantiate(mMoMFab, princesses[p].Location, Quaternion.identity) as GameObject;
				}else{
					spawn = Instantiate(eMoMFAb, princesses[p].Location, Quaternion.identity) as GameObject;
				}
				MoMController mom = spawn.GetComponent<MoMController>();
				mom.isActive = true;
				mom.SetupQueen(princesses[p]);
				princesses[p].Kill();
			}
		}else{
			KillDrones();
		}
	}

	public virtual void AddFoodLocation(Vector3 loc)
	{
		if(foodQ.Count>9)
		{
			foodQ.Dequeue();
			foodQ.Enqueue(loc);
			Debug.Log("newness");
		}else {
			qCount+=1;
			foodQ.Enqueue(loc);//Debug.Log("newness");
		}
		FoodAmount = 1;
	}

	IEnumerator UpdateLocation()
	{
		while(true)
		{
			if(foodQ.Count>0)
			MoveToCenter();
			yield return new WaitForSeconds(5);
		}
	}

	void MoveToCenter()
	{
		//Debug.Log("Updating");
		Vector3 newLoc;
		float xx = 0, zz = 0;
		int size = 1;
		for(int i = foodQ.Count; i>0;i--)
		{
			Vector3 v= foodQ.Dequeue();
			if(v != Vector3.zero)
			{
				size++;
				qCount-=1;
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
