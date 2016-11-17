using UnityEngine;
using System.Collections;

public class InputControls : MonoBehaviour 
{
	//public GameObject farmer, soldier;
	//GameObject farmFlag, fightFlag, mainMoM;
	//Transform  mainMoMTran; //farmFlagTran, fightFlagTran,;
	[SerializeField] LayerMask mask;
	[SerializeField] float speed = 10, maxFOV = 25, minFOV= 20, scrollSpeed = 3f;
	Vector3 movement;
	CameraFollow cam;

	void Start () 
	{
		cam =  GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
		//mainMoMControl = GameObject.Find("MainMoM").GetComponent<MainMomController>();
	}
	public void CreatFarmer()
	{
		MainMomController.MainMoM.CreateFarmer();
	}
	public void CreatFighter()
	{
		MainMomController.MainMoM.CreateFighter();
	}
	void Update () 
	{
		float lastInputX = Input.GetAxis ("Horizontal");
		float lastInputY = Input.GetAxis ("Vertical");
		float lastInputScroll = Input.GetAxis("Mouse ScrollWheel");
		if(lastInputX > 0f || lastInputY > 0f || lastInputX < 0f || lastInputY < 0f)
		{
			movement = new Vector3 	(speed * lastInputX,0 ,  speed * lastInputY);
			cam.MoveTo(movement);
		}

		if(lastInputScroll>0f || lastInputScroll<0f)
		{
			Camera.main.orthographicSize -= lastInputScroll* scrollSpeed;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minFOV, maxFOV);
		}

		if(MainMomController.MainMoM.isActive)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				cam.SetFollow();
			}
			if(Input.GetKeyDown(KeyCode.Q))
			{
				MainMomController.MainMoM.CreateFarmer();
			}
			if(Input.GetKeyDown(KeyCode.E))
			{
				MainMomController.MainMoM.CreateFighter();
			}
			if(Input.GetKeyDown(KeyCode.R))
			{
				MainMomController.MainMoM.CreateDaughter();
			}
			if(Input.GetKeyDown(KeyCode.Z))
			{
				MainMomController.MainMoM.RecallFarmFlag();
			}
			if(Input.GetKeyDown(KeyCode.C))
			{
				MainMomController.MainMoM.RecallFightFlag();
			}
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit, 100f, mask)) 
				{
					MainMomController.MainMoM.PlaceFarmFlag(hit.point);
				}
			}
			if (Input.GetMouseButtonDown (1)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit, 100f, mask)) 
				{
					MainMomController.MainMoM.PlaceFightFlag(hit.point);
				}
			}
		}
	}
}
