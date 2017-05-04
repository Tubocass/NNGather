using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class PlayerMomController : MoMController 
{

	bool bTeamFlag = false;
//	public static PlayerMomController MainMoM{
//		get{
//			return main;
//		}
//	}
//	static PlayerMomController main;

	protected override void Death ()
	{
		UnityEventManager.TriggerEvent("TargetUnavailable",unitID);
		//UnityEventManager.StopListeningBool("DayTime", DaySwitch);
		StopAllCoroutines();
		bMoving = false;
		farmers = 0;
		fighters = 0;
		health = startHealth;
		foodAmount = startFood;
		Foods.Clear();
		newQueen();
		daughters = 0;
		Start();
	}

	[Command]
	public void CmdCreateFarmer()
	{
		CreateFarmer();
	}
	[Command]
	public void CmdCreateFighter()
	{
		CreateFighter();
	}
	[Command]
	public void CmdCreateDaughter()
	{
		if(daughters<daughterCap)
		{
			CreateDaughter();
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
