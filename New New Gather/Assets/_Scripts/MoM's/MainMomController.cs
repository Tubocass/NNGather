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
		farmFlagFab = Instantiate(farmFlagFab) as GameObject; //GameObject.Find("FarmFlag");
		fightFlagFab = Instantiate(fightFlagFab) as GameObject;//GameObject.Find("FightFlag");
		farmFlagTran = farmFlagFab.GetComponent<Transform>();
		fightFlagTran = fightFlagFab.GetComponent<Transform>();
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
//	void Update()
//	{
//		if(Input.GetKeyDown(KeyCode.Q))
//		{
//			this.CreateFarmer();
//		}
//		if(Input.GetKeyDown(KeyCode.E))
//		{
//			//mainMoMControl.CreateFighter();
//		}
//		if(Input.GetKeyDown(KeyCode.Z))
//		{
//			RecallFarmFlag();
////			farmFlagTran.position = transform.position;
////			farmFlag.SetActive(false);
////			UnityEventManager.TriggerEvent("PlaceFarmFlag");
//		}
//		if(Input.GetKeyDown(KeyCode.C))
//		{
//			RecallFightFlag();
////			fightFlagTran.position = transform.position;
////			fightFlag.SetActive(false);
////			UnityEventManager.TriggerEvent("PlaceFightFlag");
//		}
//		if (Input.GetMouseButtonDown (0)) 
//		{
//			RaycastHit hit;
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//
//			if (Physics.Raycast (ray, out hit, 100f, mask)) 
//			{
//				PlaceFarmFlag(hit.point);
////				farmFlag.SetActive(true);
////				farmFlagTran.position = hit.point;
////				farmFlag.GetComponent<ParticleSystem>().Play();
////				UnityEventManager.TriggerEvent("PlaceFarmFlag");
//
//			}
//		}
//		if (Input.GetMouseButtonDown (1)) 
//		{
//			RaycastHit hit;
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//
//			if (Physics.Raycast (ray, out hit, 100f, mask)) 
//			{
//				PlaceFightFlag(hit.point);
////				fightFlag.SetActive(true);
////				fightFlagTran.position = hit.point;
////				fightFlag.GetComponent<ParticleSystem>().Play();
////				UnityEventManager.TriggerEvent("PlaceFightFlag");
//			}
//		}
//	}
}
