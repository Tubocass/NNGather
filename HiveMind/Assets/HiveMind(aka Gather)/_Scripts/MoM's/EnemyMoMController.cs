using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EnemyMoMController : MoMController 
{
	float timer = 0f;

	protected override void OnEnable () 
	{
		base.OnEnable();
		//if(isServer)
		//StartCoroutine(SpawnTimer());
	}

[ServerCallback]
	void Update()
	{
		timer += Time.deltaTime;
		switch((int)timer)
		{
			case 2:
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

	void MakeDecision()
	{
		/*
			
		*/
	}

//	IEnumerator SpawnTimer()
//	{
//		while(true&& isServer)
//		{
//			yield return new WaitForSeconds(1);
//			if(farmers<farmerCap)
//			{
//				CreateFarmer();
//				yield return new WaitForSeconds(1);
//			}
//		
//			if(fighters<farmers/2 && FoodAmount>2)
//			{
//				CreateFighter();
//				yield return new WaitForSeconds(2);
//			}
//
//			if(fighters + farmers>5 && daughters<=daughterCap)
//			{
//				CreateDaughter();
//				yield return new WaitForSeconds(4);
//			}
//		}
//	}

}
