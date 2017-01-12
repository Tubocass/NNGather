using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DaughterController : MoMController 
{
	[SerializeField] protected float orbit = 15;
	bool bInBloom = true;
	Vector3 birthHole;
	protected override void OnEnable () 
	{
		base.Start();
		base.OnEnable();
		farmers = 0;
		fighters = 0;
		bInBloom = true;
	}
	protected override void Start()
	{
		StopAllCoroutines();
		StartCoroutine(Bloom());
	}

	IEnumerator Bloom()
	{
		yield return new WaitForSeconds(30f);
		bInBloom = false;
		StartCoroutine(SpawnTimer());
		StartCoroutine(Hunger());
		StartCoroutine(Idle());
	}
	IEnumerator SpawnTimer()
	{
		while(true)
		{
			yield return new WaitForSeconds(1);
			if(farmers<farmerCap)
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
		birthHole = tran.position;
		GetComponentInChildren<MeshRenderer>().material.color = TeamColor;
	}
	protected override void MoveRandomly()
	{
		Vector3 rVector = RandomVector(birthHole, orbit);
		MoveTo(rVector);
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
