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
	bool bTeamFlag = false;
	public static MainMomController MainMoM{
		get{
			if(main==null)
			{
				main = FindObjectOfType<MainMomController>();
			}
			return main;
		}
	}
	static MainMomController main;

	protected override void OnEnable()
	{
		base.OnEnable();
		main = this;
		UnityEventManager.TriggerEvent("MainMomChange");
		//teamID = 0;
	}
	protected override void Start()
	{
		base.Start();
		UnityEventManager.TriggerEvent("UpdateHealth", (int)health);
		UnityEventManager.TriggerEvent("UpdateFood", foodAmount);
	}
	protected override void Death ()
	{
		base.Death();
	}
	public override void CreateFarmer()
	{
		if(farmers<farmerCap)
		{
			base.CreateFarmer();
		}
	}
	public override void CreateFighter()
	{
		if(fighters<fighterCap)
		{
			base.CreateFighter();
		}
	}
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
		if(daughters>0)
		{
			princesses = Daughters.FindAll(f=> f.isActive && f.teamID.Equals(teamID));
		}
		if(princesses.Count>0)
		{
			for(int p = 0; p<princesses.Count; p++)
			{
				princesses[p].PlaceFightFlag(location);
			}
			bTeamFlag = true;
		}
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
