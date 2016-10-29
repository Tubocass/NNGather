using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DaughterController : MoMController 
{
	protected override void OnEnable () 
	{
		base.Start();
		base.OnEnable();
		farmers = 0;
		fighters = 0;
		StartCoroutine(SpawnTimer());
	}

	IEnumerator SpawnTimer()
	{
		while(true)
		{
			yield return new WaitForSeconds(1);
			if(farmers<100)
			CreateFarmer();
			if(fighters<farmers/2 && FoodAmount>2)
			CreateFighter();
			yield return new WaitForSeconds(4);
		}
	}

	public void setMoM(MoMController mom, Color tc)
	{
		base.setMoM(mom);
		TeamColor = tc;
		GetComponentInChildren<MeshRenderer>().material.color = TeamColor;
	}
	public void Kill()//mostly used for upgrading into a MoM
	{
		Death();
	}

	protected override void Death()
	{
		base.Death();
		myMoM.daughters-=1;
	}

	protected override void newQueen()
	{
		if(myMoM.isActive)
		{
			CedeDrones(myMoM);

		}else{
			KillDrones();
		}
	}
}
