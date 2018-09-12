using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class DaughterController : MoMController 
{
	protected override void Start ()
	{
		base.Start ();
		if(isServer)
		{
			StartCoroutine(SpawnTimer());
		}
	}
	IEnumerator SpawnTimer()
	{
		while(true&& isServer)
		{
			yield return new WaitForSeconds(1);
			if(farmers<farmerCap)
			CreateFarmer();
			if(fighters<farmers/2 && FoodAmount>2)
			CreateFighter();
			yield return new WaitForSeconds(4);
		}
	}
	//[ClientRpc]
//	public override void SetMoM(GameObject mom, Color tc)
//	{
//		base.SetMoM(mom);
//		TeamColor = tc;
//	}

	public void Kill()//mostly used for upgrading into a MoM
	{
		Death();
	}

	protected override void Death()
	{
		//base.Death();
		if(myMoM.isActive)
		{
			CedeDrones(myMoM);

		}else{
			KillDrones();
		}
		myMoM.daughters-=1;
		Foods.Clear();
		UnityEventManager.TriggerEvent("TargetUnavailable",unitID);
		bMoving = false;
		isActive = false;
		hasChanged = false;
		if(teamID>=0&&GameController.instance.TeamSize[teamID]>0)
		GameController.instance.TeamSize[teamID]-=1;
	}
}
