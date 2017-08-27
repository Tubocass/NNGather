using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameController :  NetworkBehaviour
{
	public SyncListInt TeamSize = new SyncListInt();
	[SyncVar]public int numPlayers = 0;
	[SerializeField] GameObject guiFab, PlayerFab;
	[SerializeField] float SunSpeed = 2f;
	[SerializeField] float Timer = 30;
	[SyncVar]public bool bStartGame, hasGameStarted = false;
	PlayerMomController[] Players;
	GenerateLevel levelGen;
	Transform DayLight, NightLight;
	SarlacController SarlacInstance;
	bool bDay, bSinglePlayer;
	int check;

	private static GameController gameControl;
	public static GameController instance
	{
		get
		{
			if (!gameControl)
			{
				gameControl = FindObjectOfType (typeof (GameController)) as GameController;
				if (!gameControl)
				{
					Debug.LogError ("There needs to be one active GameController script on a GameObject in your scene.");
				}
			}
			return gameControl;
		}
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		if(scene.name.Equals("Main"))
		{
			DayLight = GameObject.Find("Day Light").transform;
			NightLight = GameObject.Find("Night Light").transform;
			if(bSinglePlayer)
			{
				StartNewGame();
			}
		}
    }
	void OnEnable()
	{
		DontDestroyOnLoad(this.gameObject);
		levelGen = GetComponent<GenerateLevel>();
		SceneManager.sceneLoaded += OnSceneLoaded; //this only makes sense if this object persists through scenes
		UnityEventManager.StartListeningInt("MoMDeath",IsGameOver);
		UnityEventManager.StartListening("StartGame",StartNewGame);
	}
	void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		UnityEventManager.StopListeningInt("MoMDeath",IsGameOver);
		UnityEventManager.StopListening("StartGame",StartNewGame);
	}

	public float TeamSizePercent(int t)
	{
		if(TeamSize.Count>0)
		{
			float totalPop =0;
			for(int i = 0; i< numPlayers;i++)
			{
				totalPop += TeamSize[i];
			}
			return TeamSize[t]/totalPop*100;
		}else return 0f;
	}
	void IsGameOver(int team)
	{
		if(team==0)
		{
			SceneManager.LoadScene("Score");
		}else{
			if(TeamSizePercent(0)>99.9f)
			{
				SceneManager.LoadScene("Score");
			}
		}
	}
	public bool IsDayLight()
	{
		if(DayLight!=null)
		return DayLight.eulerAngles.x>0-10&&DayLight.eulerAngles.x<180+10;
		else return false;
	}
	void Update()
	{
		if(hasGameStarted)
		{
			DayLight.Rotate(DayLight.right,SunSpeed*Time.deltaTime,Space.World);
			NightLight.Rotate(NightLight.right,SunSpeed*Time.deltaTime,Space.World);
			if(!IsDayLight()&&bDay)
			{
				bDay = false;
				UnityEventManager.TriggerEvent("DayTime",false);
				DayLight.gameObject.SetActive(false);
			}else if(IsDayLight()&&!bDay){
				bDay = true;
				UnityEventManager.TriggerEvent("DayTime",true);
				DayLight.gameObject.SetActive(true);
			}
		}
	}
	public void StartTimer()
	{
		instance.StartCoroutine(Release());
	}
	public IEnumerator Release()
	{
		yield return new WaitForSeconds(Timer);
		GameObject spawnPoint = GenerateLevel.Pits[Random.Range(0,GenerateLevel.Pits.Length-1)];
		if(spawnPoint!=null&& !GameController.instance.IsDayLight())
		{
			SarlacInstance.anchor = spawnPoint.transform.position;
			SarlacInstance.transform.position = spawnPoint.transform.position;
			SarlacInstance.isActive = true;
		}else yield return Release();

	}

	public void RegisterPlayer()
	{
		check++;
		if(check.Equals(NetworkLobbyManager.singleton.numPlayers))
		{
			StartNewGame();
		}
	}
	public void CompleteSetup(int[] array)
	{
		bSinglePlayer = true;
		SceneManager.LoadSceneAsync("Main");
		levelGen.LoadLevelSettings(array[0],array[1],array[2]);
		//StartNewGame();
		//NetworkManager.singleton.StartHost();
	}
	[Server]
	public void StartNewGame()
	{
		//Load in all rleavant info	
		if(bSinglePlayer)
		{
			ClientScene.AddPlayer(0);
		}else
		{
			check++;
			if(NetworkLobbyManager.singleton!=null&& check < NetworkLobbyManager.singleton.numPlayers)
			{
				return;
			}
		}
		Players = GameObject.FindObjectsOfType<PlayerMomController>();
		numPlayers = Players.Length+levelGen.bots;
		levelGen.Init();
		for(int t = 0; t<numPlayers; t++)
		{
			TeamSize.Add(0);
		}
		//levelGen.Init();
		levelGen.PassInPlayers(Players);
		levelGen.Generate();
		hasGameStarted = true;
		SarlacInstance = levelGen.SarlacDude.GetComponent<SarlacController>();
	

	}
}
