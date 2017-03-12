using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyMoMController : MoMController 
{
	float timer = 0f;

	// Use this for initialization
	protected override void OnEnable () 
	{
		base.Start();
		base.OnEnable();
		//StartCoroutine(SpawnTimer());
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
[ServerCallback]
	void Update()
	{
		timer += Time.deltaTime;
		switch((int)timer)
		{
			case 1:
			{
				if(farmers<farmerCap)
				CreateFarmer();
				break;
			}
			case 3:
			{	
				if(fighters<farmers/2)
				CreateFighter();
				break;
			}
			case 5:
			{
				if(fighters + farmers>5 && daughters<=daughterCap)
				CreateDaughter();
				timer = 0f;
				break;
			}
		}
	}
//	IEnumerator SpawnTimer()
//	{
//		while(true)
//		{
//			yield return new WaitForSeconds(1);
//			if(farmers<farmerCap)
//			CreateFarmer();
//			yield return new WaitForSeconds(1);
//			if(fighters<farmers/2)
//			CreateFighter();
//			yield return new WaitForSeconds(1);
//			if(fighters + farmers>5 && daughters<=daughterCap)
//			CreateDaughter();
//			yield return new WaitForSeconds(4);
//		}
//	}

}
