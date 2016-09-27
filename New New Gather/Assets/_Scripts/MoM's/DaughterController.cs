using UnityEngine;
using System.Collections;

public class DaughterController : MoMController 
{
	protected override void Start () 
	{
		base.Start();
		StartCoroutine(SpawnTimer());
	}

	IEnumerator SpawnTimer()
	{
		while(true)
		{
			yield return new WaitForSeconds(1);
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
	}
}
