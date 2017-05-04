using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Interact : NetworkBehaviour 
{
	[SerializeField] float speed = 2;
	[SerializeField] GameObject Cube;
	static int playerCount;
	[SyncVar(hook = "OnTeamChanged")] public int teamNumber;
	public Color teamColor;
	[SyncVar]public int Units = 0;
	Camera playerCam;
	[SerializeField] GameObject guiFab;
	GameObject canvas;
	TestGUI GUI;

	void Awake()
    {
        playerCam = GetComponentInChildren<Camera>();
        playerCam.gameObject.SetActive(false);
     }

	public override void OnStartLocalPlayer ()
    {
        playerCam.gameObject.SetActive(true);
        CmdSetTeam(gameObject);
		canvas = (GameObject)Instantiate(guiFab,Vector3.zero,Quaternion.identity);
        GUI = canvas.GetComponent<TestGUI>();
        GUI.playerControl = this;
    }
 
    public override void OnStartClient ()
    {
        OnTeamChanged(teamNumber);
    }

	public void OnTeamChanged(int newTeamNumber)
    {
        teamNumber = newTeamNumber;
        teamColor =  teamNumber == 0 ? Color.blue : Color.red;
		GetComponentInChildren<Renderer>().material.color = teamColor;
    }

	[Server]
    public static void SetPlayerTeam(GameObject newPlayer)
    {
        var player = newPlayer.GetComponent<Interact>();
        player.teamNumber = (int)Mathf.Repeat(playerCount, 2);
        playerCount++;
    }

	[Command]
    void CmdSetTeam(GameObject player)
    {
        SetPlayerTeam(gameObject);
    }

	[ClientCallback]
	void Update () 
	{
		if(isLocalPlayer)
		{
			float v = Input.GetAxis("Vertical");
			float h = Input.GetAxis("Horizontal");
			if(h!=0 || v!=0)
			{
				transform.Translate(new Vector3(h,0,v)* Time.deltaTime* speed);
			}

			if (Input.GetMouseButtonDown (0)) //Left Click
			{
				RaycastHit hit;
				Ray ray = GetComponentInChildren<Camera>().ScreenPointToRay (Input.mousePosition);

				if (Physics.Raycast (ray, out hit, 100f)) 
				{
					if(hit.transform.tag.Equals("Food"))
					{
						CmdChangeColor(hit.transform.gameObject);
					}
				}
			}
			if (Input.GetMouseButtonDown (1)) //Right Click
			{
				
			}
			if(Input.GetKeyDown(KeyCode.Space)) 
			{
				CmdSpawnCube();
			}
		}
	}

	[Command]
	void CmdSpawnCube()
	{
		GameObject cube = Instantiate(Cube, transform.position+transform.forward*2,Quaternion.identity) as GameObject;
		cube.GetComponent<DumbFuckingScript>().myColor = teamColor;
		Units++;
		TestGameController.instance.Count[teamNumber]+=1;
		NetworkServer.Spawn(cube);
	}

	[Command]
	void CmdChangeColor(GameObject go)
	{
		DumbFuckingScript cube = go.GetComponent<DumbFuckingScript>();
		cube.RpcChangeColor(teamColor);
	}
}
