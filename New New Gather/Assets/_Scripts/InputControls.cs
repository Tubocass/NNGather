using UnityEngine;
using System.Collections;

public class InputControls : MonoBehaviour 
{
	public GameObject farmer, soldier;
	GameObject farmFlag, fightFlag, mainMoM;
	Transform  farmFlagTran, fightFlagTran, mainMoMTran;
	[SerializeField] LayerMask mask;
	MoMController mainMoMControl;
	void Start () 
	{
		mainMoM = GameObject.Find("MainMoM");
		mainMoMTran = mainMoM.transform;
		mainMoMControl = mainMoM.GetComponent<MoMController>();
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
			farmFlagTran.position = mainMoMTran.position;
			farmFlag.SetActive(false);
			UnityEventManager.TriggerEvent("PlaceFarmFlag");
		}
		if(Input.GetKeyDown(KeyCode.C))
		{
			fightFlagTran.position = mainMoMTran.position;
			fightFlag.SetActive(false);
			UnityEventManager.TriggerEvent("PlaceFightFlag");
		}
		if (Input.GetMouseButtonDown (0)) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 100f, mask)) 
			{
				farmFlag.SetActive(true);
				farmFlagTran.position = hit.point;
				farmFlag.GetComponent<ParticleSystem>().Play();
				UnityEventManager.TriggerEvent("PlaceFarmFlag");

			}
		}
		if (Input.GetMouseButtonDown (1)) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 100f, mask)) 
			{

//				fightFlag.SetActive(true);
//				fightFlagTran.position = hit.point;
//				fightFlag.GetComponent<ParticleSystem>().Play();
//				UnityEventManager.TriggerEvent("PlaceFightFlag");
			}
		}
	}
}
