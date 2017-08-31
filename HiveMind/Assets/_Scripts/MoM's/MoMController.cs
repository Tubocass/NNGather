using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class MoMController : Unit_Base 
{
	public int FoodAmount{
		get
		{
			return foodAmount;
		}
		set
		{
			foodAmount+=value; 
			if(this.GetType()==typeof(PlayerMomController))
			{
				//UnityEventManager.TriggerEvent("UpdateFood", foodAmount);
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
	public Transform SpawnMouth;
	public List<FoodObject> Foods;
	[SyncVar]public int farmers = 0, fighters = 0, daughters = 0;//counters
	protected static List<FarmerController> Farmers = new List<FarmerController>();//object pool
	protected static List<FighterController> Fighters = new List<FighterController>();//object pool
	protected static List<DaughterController> Daughters = new List<DaughterController>();//object pool
	protected static List<MoMController> MoMs = new List<MoMController>();//object pool
	[SerializeField] protected GameObject farmerFab, fighterFab, daughterFab, eMoMFAb, mMoMFab,  farmFlagFab, fightFlagFab;
	[SerializeField] protected int farmerCost = 1, farmerCap = 42, fighterCost = 2, fighterCap = 42, daughterCost = 8, daughterCap = 6, startFood = 5, hungerTime = 10;
	[SyncVar(hook = "SetFoodUI")][SerializeField] protected int foodAmount;
	protected Transform farmFlagTran, fightFlagTran;
	protected bool activeFarmFlag, activeFightFlag;
	protected GameObject fightFlag, farmFlag;
	int qCount=0;
	Vector3 newLoc;
	Queue<Vector3> foodQ = new Queue<Vector3>(10);

	protected override void OnEnable()
	{
		base.OnEnable();

	}

//	public static int GetTeamSize(int teamNum) //function is valid, but I felt like using a static int would be cheaper
//	{
//		int myUnits = 0;
//		List<MoMController> moms = new List<MoMController>();
//		moms = MoMs.FindAll(d=> d.isActive && d.teamID==teamNum);
//
//		if(moms.Count>0)
//		{
//			foreach(MoMController d in moms)
//			{
//				myUnits += d.farmers + d.fighters + 1;
//			}
//		}
//		return myUnits;
//	}
	public void SetFoodUI(int h)
	{
		foodAmount = h;
		if(isLocalPlayer && this.GetType()==typeof(PlayerMomController))
		UnityEventManager.TriggerEvent("UpdateFood", foodAmount);
	}
	protected virtual void Start()
	{	
		TeamColorMat = GetComponentInChildren<MeshRenderer>().material;
		TeamColorMat.color = TeamColor;

		if(isServer)
		{
			Foods = new List<FoodObject>();
			if(!MoMs.Contains(this))
			MoMs.Add(this);
			health = startHealth;
			foodAmount = startFood;
			StartCoroutine(UpdateLocation());
			StartCoroutine(Hunger());
		}
	}
	protected override void Death ()
	{
		base.Death ();
		Foods.Clear();
		newQueen();
		if(GameController.instance.TeamSize[teamID]==0)
		{
			UnityEventManager.TriggerEvent("MoMDeath", teamID);
		}
	}

	public override void TakeDamage(float damage)
	{
		StartCoroutine(EmergencyFighters());
		Health = -damage/2;
	}

	protected virtual IEnumerator EmergencyFighters()
	{
		int ef = (FoodAmount - 2)/fighterCost;
		if(farmers>0 && ef>0)
		{
			for(;ef>0;ef-- )
			{
				CreateFighter();
				yield return new WaitForSeconds(0.2f);
			}
		}
	}
	protected virtual IEnumerator Hunger()
	{
		while (true)
		{
			yield return new WaitForSeconds(hungerTime);
			if(FoodAmount>=1)
			{
				FoodAmount = -1;
			}else Health = -1;
		}
	}

	[Server]
	public virtual void CreateFarmer()
	{
		if(FoodAmount>=farmerCost)
		{
			FoodAmount = -farmerCost;
			farmers++;
			GameController.instance.TeamSize[teamID] += 1;
			if(Farmers.Count>0)//Are there any farmer bots already?
			{
				FarmerController recycle = Farmers.Find(f=> !f.isActive);//are there inactive bots?
				if(recycle!=null)
				{
					//reycycle.RpsSetMom
					recycle.SetMoM(this.gameObject, TeamColor);
					recycle.transform.position = Location+new Vector3(1,0,1);
				}else{
					InstantiateFarmer();//No inactives we can recycle
				}
			}else {
				InstantiateFarmer();//No Bots available yet
			}
		}
	}

	[Server]
	public virtual void CreateFighter()
	{
		if(FoodAmount>=fighterCost)
		{
			FoodAmount = -fighterCost;
			fighters++;
			GameController.instance.TeamSize[teamID] += 1;
			if(Fighters.Count>0)
			{
				FighterController recycle = Fighters.Find(f=> !f.isActive);
				if(recycle!=null)
				{
					recycle.SetMoM(this.gameObject, TeamColor);
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
			GameController.instance.TeamSize[teamID] += 1;
			if(Daughters.Count>0)
			{
				DaughterController recycle = Daughters.Find(f=> !f.isActive);
				if(recycle!=null)
				{
					recycle.RpcSetMoM(this.gameObject, TeamColor);
					recycle.transform.position = Location+new Vector3(1,0,1);
				}else{
					InstantiateDaughter();
				}
			}else {
				InstantiateDaughter();
			}
		
		}
	}
	[Server]
	protected void InstantiateFarmer()
	{
		GameObject spawn = Instantiate(farmerFab, SpawnMouth.position, FaceForward()) as GameObject;
		FarmerController fc = spawn.GetComponent<FarmerController>();
		NetworkServer.Spawn(spawn);
		fc.SetMoM(this.gameObject, TeamColor);
		Farmers.Add(fc);
	}
	[Server]
	protected void InstantiateFighter()
	{
		GameObject spawn = Instantiate(fighterFab,SpawnMouth.position, FaceForward()) as GameObject;
		FighterController fc = spawn.GetComponent<FighterController>();
		NetworkServer.Spawn(spawn);
		fc.SetMoM(this.gameObject, TeamColor);
		Fighters.Add(fc);
	}
	[Server]
	protected void InstantiateDaughter()
	{
		GameObject spawn = Instantiate(daughterFab, SpawnMouth.position, FaceForward()) as GameObject;
		DaughterController dc = spawn.GetComponent<DaughterController>();
		NetworkServer.Spawn(spawn);
		dc.RpcSetMoM(this.gameObject, TeamColor);
		Daughters.Add(dc);
	}
	protected Quaternion FaceForward()
	{
		return Quaternion.LookRotation(SpawnMouth.position - transform.position);
	}

	public virtual void CedeDrones(MoMController newMoM)
	{
		List<FarmerController> farmTransfers = Farmers.FindAll(f=> f.isActive && f.myMoM==this);
		List<FighterController> fightTransfers = Fighters.FindAll(f=> f.isActive && f.myMoM==this);
		foreach(FarmerController f in farmTransfers)
		{
			f.SetMoM(newMoM.gameObject);
			newMoM.farmers++;
		}
		foreach(FighterController f in fightTransfers)
		{
			f.SetMoM(newMoM.gameObject);
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
			MoMController mom;
			//give first princess all drones
			CedeDrones(princesses[0]);

			for(int p = 0; p<princesses.Count; p++)
			{
				if(this.GetType() == typeof(PlayerMomController) && p==0)
				{
					//spawn = Instantiate(mMoMFab, princesses[p].Location, Quaternion.identity) as GameObject;
					tran.position = princesses[0].Location;
					princesses[0].CedeDrones(this);
				}else{
					spawn = Instantiate(eMoMFAb, princesses[p].Location, Quaternion.identity) as GameObject;
					mom = spawn.GetComponent<MoMController>();
					mom.isActive = true;
					mom.teamID = teamID;
					mom.TeamColor = TeamColor;
					princesses[p].CedeDrones(mom);
					NetworkServer.Spawn(spawn);
				}
//				mom = spawn.GetComponent<MoMController>();
//				mom.isActive = true;
//				mom.teamID = teamID;
//				mom.TeamColor = TeamColor;
//				mom.GetComponentInChildren<MeshRenderer>().material.color = TeamColor;
				princesses[p].Kill();
			}
		}else{
			KillDrones();
		}
	}

	public virtual void PlaceFarmFlag(Vector3 location)
	{
		farmFlag.SetActive(true);
		farmFlagTran.position = location;
		//farmFlagFab.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEvent("PlaceFarmFlag", unitID);
		activeFarmFlag = true;
	}
	public virtual void RecallFarmFlag()
	{
		farmFlagTran.position = transform.position;
		//farmFlag.GetComponent<ParticleSystem>().Stop();
		farmFlag.SetActive(false);
		UnityEventManager.TriggerEvent("PlaceFarmFlag", unitID);
		activeFarmFlag = false;
	}
	public virtual void PlaceFightFlag(Vector3 location)
	{
		fightFlag.SetActive(true);
		fightFlagTran.position = location;
		//fightFlag.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEvent("PlaceFightFlag", unitID);
		activeFightFlag = true;
	}
	public virtual void RecallFightFlag()
	{
		fightFlagTran.position = transform.position;
		//fightFlag.GetComponent<ParticleSystem>().Stop();
		fightFlag.SetActive(false);
		UnityEventManager.TriggerEvent("PlaceFightFlag", unitID);
		activeFightFlag = false;
	}

	public virtual void AddFoodLocation(Vector3 loc)
	{
		if(foodQ.Count>9)
		{
			foodQ.Dequeue();
			foodQ.Enqueue(loc);
		}else {
			qCount+=1;
			foodQ.Enqueue(loc);
		}
		FoodAmount = 1;
	}

	IEnumerator UpdateLocation()
	{
		while(true)
		{
			if(foodQ.Count>0)
			if(myMoM!= null && myMoM.isActive)
			{
				AddFoodLocation(myMoM.transform.position);
			}
			MoveToCenter();
			yield return new WaitForSeconds(10);
		}
	}
	[Server]
	void MoveToCenter()
	{
		float xx = Location.x, zz = Location.z;
		int size = 1;
		for(int i = foodQ.Count; i>0;i--)
		{
			Vector3 v= foodQ.Dequeue();
			if(!v.Equals( Vector3.zero))
			{
				size++;
				qCount-=1;
				xx += v.x;
				zz += v.z;
			}
		}
		newLoc = new Vector3(xx /size, 1f ,zz /size);
		if(Vector3.Distance( transform.position, newLoc)>4)
		{
			MoveTo(newLoc);
		}
	}
}
