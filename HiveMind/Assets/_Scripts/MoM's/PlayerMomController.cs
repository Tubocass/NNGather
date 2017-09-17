using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class PlayerMomController : MoMController 
{
	[SerializeField] GameObject camFab, guiFab;
	GameObject mainCam, canvas;
	GUIController GUI;
	bool bTeamFlag = false, bSetup = false;

	public override void OnStartLocalPlayer()
	{
		mainCam = (GameObject)Instantiate(camFab, transform.position + Vector3.up *30, Quaternion.identity);
		mainCam.transform.Rotate(90,0,0);
		GetComponent<InputControls>().SetCamera(mainCam.GetComponent<CameraFollow>());
		canvas = (GameObject)Instantiate(guiFab, Vector3.zero, Quaternion.identity);
		GUI = canvas.GetComponent<GUIController>();
		GUI.mainMoMControl = this;
	}
	protected override void OnEnable()
	{
		base.OnEnable();
		//TeamColor = new Color(0.765f,0.225f,0.638f,1f);
	}
	protected override void Start()
	{
		base.Start();
		UnityEventManager.TriggerEvent("MainMomChange");//Sets values in GUI
	}

	protected override void Death ()
	{
		farmFlag.SetActive(false);
		fightFlag.SetActive(false);

		bool bContinue = false;
		if(daughters>0)
		{
			bContinue = true;

		}
		base.Death();
		if(bContinue)
		{
			isActive = true;
			OnEnable();
		}
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
	{//called on client from InputController
		CmdPlaceFarmFlag(location);
		farmFlag.SetActive(true);
		farmFlagTran.position = location;
		activeFarmFlag = true;
		farmFlag.GetComponent<ParticleSystem>().Play();
	}
	[Command]
	public void CmdPlaceFarmFlag(Vector3 location)
	{
		base.PlaceFarmFlag(location);

	}
	public override void RecallFarmFlag()
	{
		base.RecallFarmFlag();
		farmFlag.GetComponent<ParticleSystem>().Stop();
	}
	public override void PlaceFightFlag(Vector3 location)
	{
		CmdPlaceFightFlag(location);
		fightFlag.SetActive(true);
		fightFlagTran.position = location;
		activeFightFlag = true;
		fightFlag.GetComponent<ParticleSystem>().Play();
	}
	[Command]
	public void CmdPlaceFightFlag(Vector3 location)
	{
		base.PlaceFightFlag(location);

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
