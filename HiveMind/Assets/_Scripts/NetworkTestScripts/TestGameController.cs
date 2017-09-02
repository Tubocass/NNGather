using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestGameController :  NetworkBehaviour
{
	public SyncListInt Count = new SyncListInt();
	[SyncVar]public int numPlayers = 0;
	Interact[] Players;

	private static TestGameController gameControl;
	public static TestGameController instance
	{
		get
		{
			if (!gameControl)
			{
				gameControl = FindObjectOfType (typeof (TestGameController)) as TestGameController;

				if (!gameControl)
				{
					Debug.LogError ("There needs to be one active TestGameController script on a GameObject in your scene.");
				}
			}
			return gameControl;
		}
	}

	void OnEnable()
	{
		DontDestroyOnLoad(this.gameObject);
		UnityEventManager.StartListening("StartGame",StartGame);
	}
	void OnDisable()
	{
		UnityEventManager.StopListening("StartGame",StartGame);
	}
	public float TeamSizePercent(int t)
	{
		float totalPop =0;
		for(int i = 0; i< numPlayers;i++)
		{
			totalPop += Count[i];
		}
		return Count[t]/totalPop*100;
	}
	[Server]
	void StartGame()
	{
		numPlayers++;
		if(numPlayers<NetworkLobbyManager.singleton.numPlayers)
		{
			return;
		}
		Players = GameObject.FindObjectsOfType<Interact>();
		for(int t = 0;t<numPlayers;t++)
		{
			Count.Add(1);
		}
	}
}
