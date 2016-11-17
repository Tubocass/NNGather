using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainMomController : MoMController 
{
//	public new float Health{
//		get
//		{
//			return health;
//		}
//		set
//		{
//			health+=value; 
//			UnityEventManager.TriggerEvent("UpdateHealth", (int)Health);
//			if(health<=0)
//			{
//				Death();
//			}
//		}
//	}
//	public new int FoodAmount{
//		get
//		{
//			return foodAmount;
//		}
//		set
//		{
//			foodAmount+=value; 
//			UnityEventManager.TriggerEvent("UpdateFood", foodAmount);
//		}
//	}

	[SerializeField] LayerMask mask;
	public static MainMomController MainMoM;

	protected override void OnEnable()
	{
		base.OnEnable();
		MainMoM = this;
		UnityEventManager.TriggerEvent("MainMomChange");
		//teamID = 0;
		farmFlagFab = Instantiate(farmFlagFab) as GameObject; 
		fightFlagFab = Instantiate(fightFlagFab) as GameObject;
		farmFlagTran = farmFlagFab.GetComponent<Transform>();
		fightFlagTran = fightFlagFab.GetComponent<Transform>();
	}
	protected override void Start()
	{
		base.Start();
		UnityEventManager.TriggerEvent("UpdateHealth", (int)health);
		UnityEventManager.TriggerEvent("UpdateFood", foodAmount);
	}
//	protected override void Death ()
//	{
//		base.Death ();
//		newQueen();
//	}
//	protected new void newQueen()
//	{
//		if(Daughters.Count>0)
//		{
//			List<DaughterController> princesses = Daughters.FindAll(f=> f.teamID==teamID && f.isActive);
//			if(princesses.Count>0)
//			{
//				GameObject spawn;
//				for(int p = 0; p<princesses.Count; p++)
//				{
//					if(p==0)
//					{
//						spawn = Instantiate(mMoMFab,princesses[p].Location, Quaternion.identity) as GameObject;
//					}else  spawn = Instantiate(eMoMFAb,princesses[p].Location, Quaternion.identity) as GameObject;;
//
//					MoMController mom = spawn.GetComponent<MoMController>();
//					List<FarmerController> farmTransfers = Farmers.FindAll(f=> f.isActive && f.myMoM==princesses[p]);
//					List<FighterController> fightTransfers = Fighters.FindAll(f=> f.isActive && f.myMoM==princesses[p]);
//					mom.SetupQueen(teamID,TeamColor, farmTransfers, fightTransfers);
//					princesses[p].isActive = false;
//				}
//			}
//		}
//	}
//	public override void CreateFarmer()
//	{
//		base.CreateFarmer();
//		UnityEventManager.TriggerEvent("UpdateFood", FoodAmount );
//	}
//	public override void CreateFighter()
//	{
//		base.CreateFighter();
//		UnityEventManager.TriggerEvent("UpdateFood", FoodAmount );
//	}
//	public override void AddFoodLocation(Vector3 loc)
//	{
//		base.AddFoodLocation(loc);
//		UnityEventManager.TriggerEvent("UpdateFood", FoodAmount);
//	}
	public virtual void PlaceFarmFlag(Vector3 location)
	{
		farmFlagFab.SetActive(true);
		farmFlagTran.position = location;
		farmFlagFab.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEvent("PlaceFarmFlag", unitID);
		activeFarmFlag = true;
	}
	public virtual void RecallFarmFlag()
	{
		farmFlagTran.position = transform.position;
		farmFlagFab.GetComponent<ParticleSystem>().Stop();
		farmFlagFab.SetActive(false);
		UnityEventManager.TriggerEvent("PlaceFarmFlag", unitID);
		activeFarmFlag = false;
	}
	public virtual void PlaceFightFlag(Vector3 location)
	{
		fightFlagFab.SetActive(true);
		fightFlagTran.position = location;
		fightFlagFab.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEvent("PlaceFightFlag", unitID);
		activeFightFlag = true;
	}
	public virtual void RecallFightFlag()
	{
		fightFlagTran.position = transform.position;
		fightFlagFab.GetComponent<ParticleSystem>().Stop();
		fightFlagFab.SetActive(false);
		UnityEventManager.TriggerEvent("PlaceFightFlag", unitID);
		activeFightFlag = false;
	}
}
