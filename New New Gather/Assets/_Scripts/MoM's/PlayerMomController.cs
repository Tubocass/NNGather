using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class PlayerMomController : MoMController 
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
	bool bTeamFlag = false;
	public static PlayerMomController MainMoM{
		get{
//			if(main==null)
//			{
//				main = FindObjectOfType<PlayerMomController>();
//			}
			return main;
		}
	}
	static PlayerMomController main;
	//Camera playerCam;

	protected override void OnEnable()
	{
		base.OnEnable();
		if(isLocalPlayer)
		{
			//playerCam.gameObject.SetActive(true);
			main = this;
			UnityEventManager.TriggerEvent("MainMomChange");
			//teamID = 0;
		}
	}
	protected override void Start()
	{
		base.Start();
		UnityEventManager.TriggerEvent("UpdateHealth", (int)health);
		UnityEventManager.TriggerEvent("UpdateFood", foodAmount);

//		playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
//		playerCam.gameObject.SetActive(false);
	}
	public override void OnStartLocalPlayer()
	{
		Debug.Log("local idiot");
		main = this;
		UnityEventManager.TriggerEvent("MainMomChange");
	}
	protected override void Death ()
	{
		base.Death();
	}

//	public override void CreateFarmer()
//	{
//		if(farmers<farmerCap)
//		{
//			base.CreateFarmer();
//		}
//	}

	[Command]
	public void CmdCreateFarmer()
	{
		if(farmers<farmerCap)
		{
			if(FoodAmount>=farmerCost)
			{
				FoodAmount = -farmerCost;
				farmers++;
				Unit_Base.TeamSize[teamID] += 1;
				if(Farmers.Count>0)
				{
					FarmerController recycle = Farmers.Find(f=> !f.isActive);
					if(recycle!=null)
					{
						//reycycle.RpsSetMom
						recycle.RpcSetMoM(this.gameObject, TeamColor);
						recycle.transform.position = Location+new Vector3(1,0,1);
					}else{
						InstantiateFarmer();
						//CmdInstantiateFarmer();
					}
				}else {
					InstantiateFarmer();
					//CmdInstantiateFarmer();
				}
			}
		}
	}
//	[Command]
//	public void CmdInstantiateFarmer()
//	{
//		GameObject spawn = Instantiate(farmerFab, Location + new Vector3(1,0,-1),Quaternion.identity) as GameObject;
//		FarmerController fc = spawn.GetComponent<FarmerController>();
//		NetworkServer.Spawn(spawn);
//		fc.RpcSetMoM(this.gameObject, TeamColor);
//		Farmers.Add(fc);
//
//	}
//	public override void CreateFighter()
//	{
//		if(fighters<fighterCap)
//		{
//			base.CreateFighter();
//		}
//	}
	[Command]
	public void CmdCreateFighter()
	{
		if(fighters<fighterCap)
		{
			if(FoodAmount>=fighterCost)
			{
				FoodAmount = -fighterCost;
				fighters++;
				Unit_Base.TeamSize[teamID] += 1;
				if(Fighters.Count>0)
				{
					FighterController recycle = Fighters.Find(f=> !f.isActive);
					if(recycle!=null)
					{
						//reycycle.RpsSetMom
						recycle.RpcSetMoM(this.gameObject, TeamColor);
						recycle.transform.position = Location+new Vector3(1,0,1);
					}else{
						InstantiateFighter();
						//CmdInstantiateFighter();
					}
				}else {
					InstantiateFighter();
					//CmdInstantiateFighter();
				}
			}
		}
	}
//	[Command]
//	public void CmdInstantiateFighter()
//	{
//		GameObject spawn = Instantiate(fighterFab, Location + new Vector3(1,0,-1),Quaternion.identity) as GameObject;
//		FighterController fc = spawn.GetComponent<FighterController>();
//		NetworkServer.Spawn(spawn);
//		fc.RpcSetMoM(this.gameObject, TeamColor);
//		Fighters.Add(fc);
//
//	}
	public override void CreateDaughter()
	{
		if(daughters<daughterCap)
		{
			base.CreateDaughter();
		}
	}
	public override void PlaceFarmFlag(Vector3 location)
	{
		base.PlaceFarmFlag(location);
		farmFlag.GetComponent<ParticleSystem>().Play();
	}
	public override void RecallFarmFlag()
	{
		base.RecallFarmFlag();
		farmFlag.GetComponent<ParticleSystem>().Stop();
	}
	public override void PlaceFightFlag(Vector3 location)
	{
		base.PlaceFightFlag(location);
		fightFlag.GetComponent<ParticleSystem>().Play();
	}
	public override void RecallFightFlag()
	{

		base.RecallFightFlag();
		fightFlag.GetComponent<ParticleSystem>().Stop();
	
		if(bTeamFlag)
		RecallTeamFightFlag();
	}
	public void PlaceTeamFightFlag(Vector3 location)
	{
		base.PlaceFightFlag(location);
		fightFlag.GetComponent<ParticleSystem>().Play();
		List<DaughterController> princesses = new List<DaughterController>();
		List<MoMController> otherMoMs = new List<MoMController>();
		otherMoMs = MoMs.FindAll(f=> f.isActive && f.teamID == teamID && f.unitID != unitID);
		if(daughters>0)
		{
			princesses = Daughters.FindAll(f=> f.isActive && f.teamID == teamID);
		}
		if(princesses.Count>0)
		{
			for(int p = 0; p<princesses.Count; p++)
			{
				princesses[p].PlaceFightFlag(location);
			}
		}
		if(otherMoMs.Count>0)
		{
			for(int p = 0; p<otherMoMs.Count; p++)
			{
				otherMoMs[p].PlaceFightFlag(location);
			}
		}
		bTeamFlag = true;
	}
	public void RecallTeamFightFlag()
	{
		bTeamFlag = false;
		List<DaughterController> princesses = new List<DaughterController>();
		if(daughters>0)
		{
			princesses = Daughters.FindAll(f=> f.isActive && f.myMoM==this);
		}
		if(princesses.Count>0)
		{
			for(int p = 0; p<princesses.Count; p++)
			{
				princesses[p].RecallFightFlag();
			}
		}
	}
}
