using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class InputControls :  NetworkBehaviour
{
	[SerializeField] LayerMask mask;
	[SerializeField] float speed = 10, maxFOV = 25, minFOV= 20, scrollSpeed = 3f;
	Vector3 movement;
	CameraFollow camFollow;
	PlayerMomController playerMoM;

	void Awake()
	{
		playerMoM = GetComponent<PlayerMomController>();//make sure this script is attached to player
	}
	public void SetCamera(CameraFollow cam)
	{
		camFollow = cam;
		camFollow.SetTarget(transform);//make sure this script is attached to player
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
