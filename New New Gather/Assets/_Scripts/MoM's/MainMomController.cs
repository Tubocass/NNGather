using UnityEngine;
using System.Collections;

public class MainMomController : MoMController 
{
	[SerializeField] LayerMask mask;

	protected override void OnEnable()
	{
		base.OnEnable();
		teamID = 0;
		UnityEventManager.TriggerEvent("UpdateFood", FoodAmount);
		farmFlagFab = Instantiate(farmFlagFab) as GameObject; 
		fightFlagFab = Instantiate(fightFlagFab) as GameObject;
		farmFlagTran = farmFlagFab.GetComponent<Transform>();
		fightFlagTran = fightFlagFab.GetComponent<Transform>();
	}

	public override void CreateFarmer()
	{
		base.CreateFarmer();
		UnityEventManager.TriggerEvent("UpdateFood", FoodAmount );
	}
	public override void CreateFighter()
	{
		base.CreateFighter();
		UnityEventManager.TriggerEvent("UpdateFood", FoodAmount );
	}
	public override void AddFoodLocation(Vector3 loc)
	{
		base.AddFoodLocation(loc);
		UnityEventManager.TriggerEvent("UpdateFood", FoodAmount);
	}
	public virtual void PlaceFarmFlag(Vector3 location)
	{
		farmFlagFab.SetActive(true);
		farmFlagTran.position = location;
		farmFlagFab.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEvent("PlaceFarmFlag", teamID);
		activeFarmFlag = true;
	}
	public virtual void RecallFarmFlag()
	{
		farmFlagTran.position = transform.position;
		farmFlagFab.GetComponent<ParticleSystem>().Stop();
		farmFlagFab.SetActive(false);
		UnityEventManager.TriggerEvent("PlaceFarmFlag", teamID);
		activeFarmFlag = false;
	}
	public virtual void PlaceFightFlag(Vector3 location)
	{
		fightFlagFab.SetActive(true);
		fightFlagTran.position = location;
		fightFlagFab.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEvent("PlaceFightFlag", teamID);
		activeFightFlag = true;
	}
	public virtual void RecallFightFlag()
	{
		fightFlagTran.position = transform.position;
		fightFlagFab.GetComponent<ParticleSystem>().Stop();
		fightFlagFab.SetActive(false);
		UnityEventManager.TriggerEvent("PlaceFightFlag", teamID);
		activeFightFlag = false;
	}
}
