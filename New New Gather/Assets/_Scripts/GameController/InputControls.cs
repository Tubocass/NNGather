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
	PlayerMomController playerMoM;

	void Awake()
	{
		//Camera.main.gameObject.SetActive(false);
	}
	void Start () 
	{

	}
	public override void OnStartLocalPlayer ()
    {
		cam =  GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
		//cam.gameObject.SetActive(false);
		GUI = GameObject.FindObjectOfType<GUIController>();
		playerMoM = GetComponent<PlayerMomController>();
    }
//	public override void OnStartClient ()
//    {
//		OnTeamChanged(playerMoM.teamID);
//    }
//
//	public void OnTeamChanged(int newTeamNumber)
//    {
//		playerMoM.teamID = newTeamNumber;
//		playerMoM.TeamColor =  playerMoM.teamID == 0 ? Color.blue : Color.red;
//		GetComponentInChildren<Renderer>().material.color = teamColor;
//    }
//
//	[Server]
//    public static void SetPlayerTeam(GameObject newPlayer)
//    {
//        var player = newPlayer.GetComponent<Interact>();
//        player.teamNumber = (int)Mathf.Repeat(playerCount, 2);
//        playerCount++;
//    }
//
//	[Command]
//    void CmdSetTeam(GameObject player)
//    {
//        SetPlayerTeam(gameObject);
//    }
//	public void CreatFarmer()//UI hooks
//	{
//		playerMoM.CreateFarmer();
//	}
//	public void CreatFighter()
//	{
//		playerMoM.CreateFighter();
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
		if(lastInputX != 0f || lastInputY != 0f)
		{
			movement = new Vector3 	(speed * lastInputX,0 ,  speed * lastInputY);
			cam.MoveTo(movement);
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
				cam.SetFollow();
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
