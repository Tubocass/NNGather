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
	public int maxFood = 20;
	[SyncVar]public int farmers = 0, fighters = 0, daughters = 0;//counters
	protected static List<FarmerController> FarmerPool = new List<FarmerController>();//object pool
	protected static List<FighterController> FighterPool = new List<FighterController>();//object pool
	protected static List<DaughterController> DaughterPool = new List<DaughterController>();//object pool
	protected static List<MoMController> MoMPool = new List<MoMController>();//object pool
	[SerializeField] protected GameObject farmerFab, fighterFab, daughterFab, eMoMFAb, mMoMFab,  farmFlagFab, fightFlagFab;
	[SerializeField] protected int farmerCost = 1, farmerCap = 42, fighterCost = 2, fighterCap = 42, daughterCost, daughterCap = 6, startFood = 5, hungerTime = 10;
	[SyncVar(hook = "SetFoodUI")][SerializeField] protected int foodAmount;
	protected GameObject fightFlag, farmFlag;
	protected Transform farmFlagTran, fightFlagTran;
	protected bool activeFarmFlag, activeFightFlag;
	protected Vector3 foodMidpoint;
	Vector3 SpawnMouth{get{return tran.TransformPoint(new Vector3(1f,0,0));}}
	Quaternion FaceForward{get{return Quaternion.LookRotation(SpawnMouth - transform.position);}}
	int qCount=0;
	Queue<Vector3> foodQ = new Queue<Vector3>(10);

	void Awake()
	{
		farmFlag = Instantiate(farmFlagFab) as GameObject; 
		fightFlag = Instantiate(fightFlagFab) as GameObject;
		farmFlagTran = farmFlag.GetComponent<Transform>();
		fightFlagTran = fightFlag.GetComponent<Transform>();
	}
	protected override void OnEnable()
	{
		base.OnEnable();
		foodAmount = startFood;
	}

	protected override void Start()
	{	
		base.Start();
		TeamColorMat = GetComponentInChildren<MeshRenderer>().material;
		TeamColorMat.color = TeamColor;
		if(isServer)
		{
			Foods = new List<FoodObject>();
			if(!MoMPool.Contains(this))
			MoMPool.Add(this);
			StartCoroutine(UpdateLocation());
			StartCoroutine(Hunger());
		}
	}

	public void SetFoodUI(int h)
	{
		foodAmount = h;
		if(isLocalPlayer && this.GetType()==typeof(PlayerMomController))
		UnityEventManager.TriggerEvent("UpdateFood", foodAmount);
	}

	protected override void Death ()
	{
		base.Death ();
		Foods.Clear();
		newQueen();
		if(NewGameController.Instance.TeamSize[teamID]==0)
		{
			UnityEventManager.TriggerEvent("MoMDeath", teamID);//sends notification to GUI
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
            NewGameController.Instance.TeamSize[teamID] += 1;
			if(FarmerPool.Count>0)//Are there any farmer bots already?
			{
				FarmerController recycle = FarmerPool.Find(f=> !f.isActive);//are there inactive bots?
				if(recycle!=null)
				{
					//reycycle.RpsSetMom
					recycle.SetMoM(this.gameObject);
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
            NewGameController.Instance.TeamSize[teamID] += 1;
			if(FighterPool.Count>0)
			{
				FighterController recycle = FighterPool.Find(f=> !f.isActive);
				if(recycle!=null)
				{
					recycle.SetMoM(this.gameObject);
					recycle.transform.position = Location+new Vector3(1,0,1);
				}else{
					InstantiateFighter();
				}
			}else {
				InstantiateFighter();
			}
		}
	}
	[Server]
	public virtual void CreateDaughter()
	{
		if(FoodAmount>=maxFood)
		{
			FoodAmount = -maxFood;
			daughters++;
            NewGameController.Instance.TeamSize[teamID] += 1;
			if(DaughterPool.Count>0)
			{
				DaughterController recycle = DaughterPool.Find(f=> !f.isActive);
				if(recycle!=null)
				{
					recycle.SetMoM(this.gameObject);
					recycle.transform.position = Location+new Vector3(1,0,1);
				}else{
					InstantiateDaughter();
				}
			}else {
				InstantiateDaughter();
			}
			maxFood +=10;
		}
	}
	[Server]
	protected void InstantiateFarmer()
	{
		GameObject spawn = Instantiate(farmerFab, SpawnMouth, FaceForward) as GameObject;
		FarmerController fc = spawn.GetComponent<FarmerController>();
		NetworkServer.Spawn(spawn);
		fc.SetMoM(this.gameObject);
		FarmerPool.Add(fc);
	}
	[Server]
	protected void InstantiateFighter()
	{
		GameObject spawn = Instantiate(fighterFab,SpawnMouth, FaceForward) as GameObject;
		FighterController fc = spawn.GetComponent<FighterController>();
		NetworkServer.Spawn(spawn);
		fc.SetMoM(this.gameObject);
		FighterPool.Add(fc);
	}
	[Server]
	protected void InstantiateDaughter()
	{
		GameObject spawn = Instantiate(daughterFab, SpawnMouth, FaceForward) as GameObject;
		DaughterController dc = spawn.GetComponent<DaughterController>();
		NetworkServer.Spawn(spawn);
		dc.SetMoM(this.gameObject);
		DaughterPool.Add(dc);
	}

	public virtual void CedeDrones(MoMController newMoM)
	{
		List<FarmerController> farmTransfers = FarmerPool.FindAll(f=> f.isActive && f.myMoM==this);
		List<FighterController> fightTransfers = FighterPool.FindAll(f=> f.isActive && f.myMoM==this);
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
		List<FarmerController> farmTransfers = FarmerPool.FindAll(f=> f.isActive && f.myMoM==this);
		List<FighterController> fightTransfers = FighterPool.FindAll(f=> f.isActive && f.myMoM==this);
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
		if(DaughterPool.Count>0)
		{
			princesses = DaughterPool.FindAll(f=> f.isActive && f.myMoM==this);
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
		activeFarmFlag = true;
		UnityEventManager.TriggerEvent("PlaceFarmFlag", unitID);
	}
	public virtual void RecallFarmFlag()
	{
		farmFlagTran.position = transform.position;
		farmFlag.SetActive(false);
		activeFarmFlag = false;
		UnityEventManager.TriggerEvent("PlaceFarmFlag", unitID);
	}
	public virtual void PlaceFightFlag(Vector3 location)
	{
		fightFlag.SetActive(true);
		fightFlagTran.position = location;
		activeFightFlag = true;
		UnityEventManager.TriggerEvent("PlaceFightFlag", unitID);
	}
	public virtual void RecallFightFlag()
	{
		fightFlagTran.position = transform.position;
		fightFlag.SetActive(false);
		activeFightFlag = false;
		UnityEventManager.TriggerEvent("PlaceFightFlag", unitID);
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
		if(FoodAmount<maxFood)
		FoodAmount = 1;
	}

	IEnumerator UpdateLocation()
	{
		while(true)
		{
			if(foodQ.Count>0)
			{
				if(myMoM!= null && myMoM.isActive)
				{
					AddFoodLocation(myMoM.transform.position);
				}
				MoveToCenter();
			}
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
		foodMidpoint = new Vector3(xx /size, 1f ,zz /size);
		if(Vector3.Distance( transform.position, foodMidpoint)>4)
		{
			MoveTo(foodMidpoint);
		}
	}
}
