using UnityEngine;
using System.Collections;

public class InputControls : MonoBehaviour 
{
	//public GameObject farmer, soldier;
	//GameObject farmFlag, fightFlag, mainMoM;
	//Transform  mainMoMTran; //farmFlagTran, fightFlagTran,;
	[SerializeField] LayerMask mask;
	[SerializeField] float speed = 10;
	MainMomController mainMoMControl;
	Vector3 movement;
	CameraFollow cam;


	void OnEnable()
	{
		UnityEventManager.StartListening("MainMomChange",MoMChanged);
	}
	void OnDisable()
	{
		UnityEventManager.StopListening("MainMomChange",MoMChanged);
	}
	void Start () 
	{
		cam =  GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
		//mainMoMControl = GameObject.Find("MainMoM").GetComponent<MainMomController>();
	}
	void MoMChanged(GameObject Main)
	{
		mainMoMControl = Main.GetComponent<MainMomController>();
		cam.SetTarget(mainMoMControl.transform);
	}
	void Update () 
	{
		float lastInputX = Input.GetAxis ("Horizontal");
		float lastInputY = Input.GetAxis ("Vertical");
		if(lastInputX > 0f || lastInputY > 0f || lastInputX < 0f || lastInputY < 0f)
		{
			movement = new Vector3 	(speed * lastInputX,0 ,  speed * lastInputY);
			cam.MoveTo(movement);
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			cam.SetFollow();
		}
		if(Input.GetKeyDown(KeyCode.Q))
		{
			mainMoMControl.CreateFarmer();
		}
		if(Input.GetKeyDown(KeyCode.E))
		{
			mainMoMControl.CreateFighter();
		}
		if(Input.GetKeyDown(KeyCode.R))
		{
			mainMoMControl.CreateDaughter();
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
