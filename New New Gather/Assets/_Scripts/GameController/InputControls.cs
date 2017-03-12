using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class InputControls :  NetworkBehaviour
{
	//public GameObject farmer, soldier;
	//GameObject farmFlag, fightFlag, mainMoM;
	//Transform  mainMoMTran; //farmFlagTran, fightFlagTran,;
	[SerializeField] LayerMask mask;
	[SerializeField] float speed = 10, maxFOV = 25, minFOV= 20, scrollSpeed = 3f;
	Vector3 movement;
	CameraFollow cam;
	GUIController GUI;

	void Start () 
	{
		cam =  GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
		GUI = GameController.instance.GetComponent<GUIController>();
		//mainMoMControl = GameObject.Find("MainMoM").GetComponent<MainMomController>();
	}
//	public void CreatFarmer()//UI hooks
//	{
//		PlayerMomController.MainMoM.CreateFarmer();
//	}
//	public void CreatFighter()
//	{
//		PlayerMomController.MainMoM.CreateFighter();
//	}
	void Update () 
	{
		if(!isLocalPlayer)
		{
			return;
		}
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

		if(PlayerMomController.MainMoM!=null && PlayerMomController.MainMoM.isActive)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				cam.SetFollow();
			}
			if(Input.GetKeyDown(KeyCode.Q))
			{
				//PlayerMomController.MainMoM.CreateFarmer();
				PlayerMomController.MainMoM.CmdCreateFarmer();
			}
			if(Input.GetKeyDown(KeyCode.E))
			{
				PlayerMomController.MainMoM.CmdCreateFighter();
			}
			if(Input.GetKeyDown(KeyCode.R))
			{
				PlayerMomController.MainMoM.CreateDaughter();
			}
			if(Input.GetKeyDown(KeyCode.Z))
			{
				PlayerMomController.MainMoM.RecallFarmFlag();
			}
			if(Input.GetKeyDown(KeyCode.C))
			{
				PlayerMomController.MainMoM.RecallFightFlag();
			}
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit, 100f, mask)) 
				{
					PlayerMomController.MainMoM.PlaceFarmFlag(hit.point);
				}
			}
			if (Input.GetMouseButtonDown (1)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit, 100f, mask)) 
				{
					if(Input.GetKey(KeyCode.LeftShift))
					{
						Debug.Log("Super");
						PlayerMomController.MainMoM.PlaceTeamFightFlag(hit.point);
					}
					else PlayerMomController.MainMoM.PlaceFightFlag(hit.point);
				}
			}
		}
	}
}
