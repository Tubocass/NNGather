using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DaughterController : MoMController 
{
	[SerializeField] protected float orbit = 15, delayStart = 15f;
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
		//StopAllCoroutines();
		//StartCoroutine(Bloom());
		StartCoroutine(SpawnTimer());
		StartCoroutine(Hunger());
	}

//	IEnumerator Bloom()
//	{
//		yield return new WaitForSeconds(delayStart);
//		bInBloom = false;
//		StartCoroutine(SpawnTimer());
//		StartCoroutine(Hunger());
//		//StartCoroutine(Idle());
//	}
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

	public void RpcSetMoM(GameObject mom, Color tc)
	{
		base.SetMoM(mom);
		TeamColor = tc;
		birthHole = tran.position;
		GetComponentInChildren<MeshRenderer>().material.color = TeamColor;
	}
//	protected override void MoveRandomly()
//	{
//		UnityEngine.AI.NavMeshPath rVector = RandomPath(birthHole, orbit);
//		RpcMoveTo(rVector.corners);
//	}
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
