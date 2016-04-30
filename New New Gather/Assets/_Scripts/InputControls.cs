using UnityEngine;
using System.Collections;

public class InputControls : MonoBehaviour 
{
	public GameObject farmer, soldier;
	GameObject farmFlag, fightFlag, mainMoM;
	Transform  farmFlagTran, fightFlagTran, mainMoMTran;
	[SerializeField] LayerMask mask;
	MainMomController mainMoMControl;
	void Start () 
	{
		mainMoM = GameObject.Find("MainMoM");
		mainMoMTran = mainMoM.transform;
		mainMoMControl = mainMoM.GetComponent<MainMomController>();
		farmFlag = GameObject.Find("FarmFlag");
		fightFlag = GameObject.Find("FightFlag");
		farmFlagTran = farmFlag.GetComponent<Transform>();
		fightFlagTran = fightFlag.GetComponent<Transform>();
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
			mainMoMControl.CreateFarmer();
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			mainMoMControl.CreateFighter();
		}
		if(Input.GetKeyDown(KeyCode.Z))
		{
			mainMoMControl.RecallFarmFlag();
		}
		if(Input.GetKeyDown(KeyCode.C))
		{
			mainMoMControl.RecallFightFlag();
		}
		if (Input.GetMouseButtonDown (0)) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 100f, mask)) 
			{
				mainMoMControl.PlaceFarmFlag(hit.point);
			}
		}
		if (Input.GetMouseButtonDown (1)) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 100f, mask)) 
			{
				mainMoMControl.PlaceFightFlag(hit.point);
			}
		}
	}
}
