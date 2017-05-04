using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class InputControls :  NetworkBehaviour
{
	[SerializeField] LayerMask mask;
	[SerializeField] float speed = 10, maxFOV = 25, minFOV= 20, scrollSpeed = 3f;
	Vector3 movement;
	[SerializeField] GameObject camFab, guiFab;
	GameObject mainCam, canvas;
	CameraFollow camFollow;
	GUIController GUI;
	PlayerMomController playerMoM;

//	void OnEnable ()
//    {
//		SceneManager.sceneLoaded += OnSceneLoaded;
//    }
//    void OnDisable()
//    {
//		SceneManager.sceneLoaded -= OnSceneLoaded;
//    }
//
//    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//    {
//    	if(isLocalPlayer)
//    	{
//			
//    	}
//    }
	void Awake()
	{
		playerMoM = GetComponent<PlayerMomController>();
		mainCam = (GameObject)Instantiate(camFab, transform.position + Vector3.up *30, Quaternion.identity);
		mainCam.transform.Rotate(90,0,0);
		mainCam.SetActive(false);
	}
	public void Start()
	{
		if(isLocalPlayer)
		{
			mainCam.SetActive(true);
			camFollow = mainCam.GetComponent<CameraFollow>();
			camFollow.SetTarget(playerMoM.transform);
			canvas = (GameObject)Instantiate(guiFab, Vector3.zero, Quaternion.identity);
			GUI = canvas.GetComponent<GUIController>();
			GUI.mainMoMControl = playerMoM;
			UnityEventManager.TriggerEvent("UpdateHealth", (int)playerMoM.Health);
			UnityEventManager.TriggerEvent("UpdateFood", playerMoM.FoodAmount);
		}

//		GUI.SetTeams(GameController.instance.numPlayers);
//		NetworkServer.Spawn(canvas);
//		NetworkServer.Spawn(mainCam);
		//UnityEventManager.TriggerEvent("MainMomChange");
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
				playerMoM.CmdCreateFarmer();
			}
			if(Input.GetKeyDown(KeyCode.E))
			{
				playerMoM.CmdCreateFighter();
			}
			if(Input.GetKeyDown(KeyCode.R))
			{
				playerMoM.CmdCreateDaughter();
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
