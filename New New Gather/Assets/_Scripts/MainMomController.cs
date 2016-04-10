using UnityEngine;
using System.Collections;

public class MainMomController : MoMController 
{
	[SerializeField] LayerMask mask;

	protected override void OnEnable()
	{
		base.OnEnable();
		UnityEventManager.TriggerEventInt("UpdateFood", FoodAmount);
	}
	protected override void SetID()
	{
		TeamID = 0;
	}
//	protected override void PlaceFarmFlag(Vector3 location)
//	{
//		activeFarmFlag = true;
//	}
//	protected override void RecallFarmFlag(Vector3 location)
//	{
//		activeFarmFlag = false;
//	}
	public override void CreateFarmer()
	{
		base.CreateFarmer();
		UnityEventManager.TriggerEventInt("UpdateFood", FoodAmount );
	}
	public override void CreateFighter()
	{
		base.CreateFighter();
		UnityEventManager.TriggerEventInt("UpdateFood", FoodAmount );
	}
	public override void AddFoodLocation(Vector3 loc)
	{
		base.AddFoodLocation(loc);
		UnityEventManager.TriggerEventInt("UpdateFood", FoodAmount);
	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
			CreateFarmer();
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			//mainMoMControl.CreateFighter();
		}
		if(Input.GetKeyDown(KeyCode.Z))
		{
			RecallFarmFlag();
//			farmFlagTran.position = transform.position;
//			farmFlag.SetActive(false);
//			UnityEventManager.TriggerEvent("PlaceFarmFlag");
		}
		if(Input.GetKeyDown(KeyCode.C))
		{
			RecallFightFlag();
//			fightFlagTran.position = transform.position;
//			fightFlag.SetActive(false);
//			UnityEventManager.TriggerEvent("PlaceFightFlag");
		}
		if (Input.GetMouseButtonDown (0)) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 100f, mask)) 
			{
				PlaceFarmFlag(hit.point);
//				farmFlag.SetActive(true);
//				farmFlagTran.position = hit.point;
//				farmFlag.GetComponent<ParticleSystem>().Play();
//				UnityEventManager.TriggerEvent("PlaceFarmFlag");

			}
		}
		if (Input.GetMouseButtonDown (1)) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 100f, mask)) 
			{
				PlaceFightFlag(hit.point);
//				fightFlag.SetActive(true);
//				fightFlagTran.position = hit.point;
//				fightFlag.GetComponent<ParticleSystem>().Play();
//				UnityEventManager.TriggerEvent("PlaceFightFlag");
			}
		}
	}
}
