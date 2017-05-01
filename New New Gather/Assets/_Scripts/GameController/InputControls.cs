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
	[SerializeField] GameObject camFab;
	GameObject mainCam;
	CameraFollow camFollow;
	GUIController GUI;
	PlayerMomController playerMoM;


	public override void OnStartClient ()
    {
		playerMoM = GetComponent<PlayerMomController>();
		GUI = GameObject.FindObjectOfType<GUIController>();
    }
	public override void OnStartLocalPlayer()
	{
		mainCam = (GameObject)Instantiate(camFab, transform.position + Vector3.up *30, Quaternion.identity);
		mainCam.transform.Rotate(90,0,0);
		camFollow =  mainCam.GetComponent<CameraFollow>();
		camFollow.SetTarget(playerMoM.transform);
		camFollow.SetFollow();
	}
	void Update () 
	{
		if(!isLocalPlayer)
		{
			return;
		}
		float lastInputX = Input.GetAxis ("Horizontal");
		float lastInputY = Input.GetAxis ("Vertical");
		float lastInputScroll = Input.GetAxis("Mouse ScrollWheel");
		if(lastInputX != 0f || lastInputY != 0f)
		{
			movement = new Vector3 	(speed * lastInputX,0 ,  speed * lastInputY);
			camFollow.MoveTo(movement);
		}

		if(lastInputScroll>0f || lastInputScroll<0f)
		{
			Camera.main.orthographicSize -= lastInputScroll* scrollSpeed;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minFOV, maxFOV);
		}

		if(playerMoM!=null && playerMoM.isActive)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				camFollow.SetFollow();
			}
			if(Input.GetKeyDown(KeyCode.Q))
			{
				//playerMoM.CreateFarmer();
				playerMoM.CmdCreateFarmer();
			}
			if(Input.GetKeyDown(KeyCode.E))
			{
				playerMoM.CmdCreateFighter();
			}
			if(Input.GetKeyDown(KeyCode.R))
			{
				playerMoM.CreateDaughter();
			}
			if(Input.GetKeyDown(KeyCode.Z))
			{
				playerMoM.RecallFarmFlag();
			}
			if(Input.GetKeyDown(KeyCode.C))
			{
				playerMoM.RecallFightFlag();
			}
			if (Input.GetMouseButtonDown (0)) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit, 100f, mask)) 
				{
					playerMoM.PlaceFarmFlag(hit.point);
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
						playerMoM.PlaceTeamFightFlag(hit.point);
					}
					else playerMoM.PlaceFightFlag(hit.point);
				}
			}
		}
	}
}
