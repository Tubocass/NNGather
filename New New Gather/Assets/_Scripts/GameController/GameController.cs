using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameController :  NetworkBehaviour
{
	//public static List<GameObject> Pits = new List<GameObject>();
	static Transform DayLight, NightLight;
	static float timer;
	static SarlacController SarlacInstance;
	[SerializeField] GameObject SarlacFab;
	[SerializeField] float SunSpeed = 2f;
	[SerializeField] float Timer = 30;
	[SerializeField] bool bStartGame;
	GenerateLevel levelGen;
	bool bDay;

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

	void OnEnable()
	{
		UnityEventManager.StartListeningInt("MoMDeath",IsGameOver);
	}
	void OnDisable()
	{
		UnityEventManager.StopListeningInt("MoMDeath",IsGameOver);
	}

	void Start()
	{
		if(!isServer)
		return;

		levelGen = GetComponent<GenerateLevel>();
		DayLight = GameObject.Find("Day Light").transform;
		NightLight = GameObject.Find("Night Light").transform;
		if(bStartGame)
		//levelGen.Generate();
		timer = Timer;
		if(SarlacInstance == null)
		{
			GameObject spawn = Instantiate(SarlacFab, Vector3.zero,Quaternion.identity) as GameObject;
			SarlacInstance = spawn.GetComponent<SarlacController>();
			SarlacInstance.isActive = false;
			NetworkServer.Spawn(spawn);
			StartCoroutine(Release());
		}
	}
	public static float TeamSizePercent(int t)
	{
		float totalPop =0;
		int teams =  instance.levelGen.moms;
		for(int i = 0; i< teams;i++)
		{
			totalPop +=Unit_Base.TeamSize[i];
		}
		return Unit_Base.TeamSize[t]/totalPop*100;
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

	void Update()
	{
		if(!isServer)
		return;

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
	public static bool IsDayLight()
	{
		if(DayLight!=null)
		return DayLight.eulerAngles.x>0-10&&DayLight.eulerAngles.x<180+10;
		else return false;
	}

	public static void StartTimer()
	{
		instance.StartCoroutine(Release());
	}
	public static IEnumerator Release()
	{
		yield return new WaitForSeconds(timer);
		GameObject spawnPoint = GenerateLevel.Pits[Random.Range(0,GenerateLevel.Pits.Length-1)];
		if(spawnPoint!=null&& !GameController.IsDayLight())
		{
			SarlacInstance.anchor = spawnPoint.transform.position;
			SarlacInstance.transform.position = spawnPoint.transform.position;
			SarlacInstance.isActive = true;
		}else yield return Release();

	}

	public void StartNewGame()
	{
		//SceneManager.LoadScene("Main");
		levelGen.Generate();
	}
}
