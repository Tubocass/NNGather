using UnityEngine;
using System.Collections;

public class EnemyMoMController : MoMController 
{

	// Use this for initialization
	protected override void OnEnable () 
	{
		base.Start();
		base.OnEnable();
		StartCoroutine(SpawnTimer());
	}
//	protected override void Death ()
//	{
//		base.Death ();
//		newQueen();
//	}
//	public override void CreateFarmer()
//	{
//		if(FoodAmount>0)
//		{
//			FoodAmount -= 1;
//			//UnityEventManager.TriggerEventInt("UpdateFood", FoodAmount);
//			GameObject spawn = Instantiate(farmer,transform.position + new Vector3(1,0,1),Quaternion.identity) as GameObject;
//			spawn.GetComponent<FarmerController>().setMoM(transform, transform);
//		}
//	}
	IEnumerator SpawnTimer()
	{
		while(true)
		{
			yield return new WaitForSeconds(1);
			if(farmers<100)
			CreateFarmer();
			yield return new WaitForSeconds(1);
			if(fighters<farmers/2)
			CreateFighter();
			yield return new WaitForSeconds(1);
			if(fighters + farmers>5 && daughters<=5)
			CreateDaughter();
			yield return new WaitForSeconds(4);
		}
	}

}
