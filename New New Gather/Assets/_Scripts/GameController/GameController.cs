using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour 
{
	[SerializeField] float SunSpeed = 2f;
	static Transform DayLight, NightLight;
	GenerateLevel levelGen;
	bool bDay;

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
		levelGen = GetComponent<GenerateLevel>();
		DayLight = GameObject.Find("Day Light").transform;
		NightLight = GameObject.Find("Night Light").transform;
		levelGen.Generate();
	}

	void IsGameOver(int team)
	{
		if(team==0)
		{
			SceneManager.LoadScene("Score");
		}else{
			float t1 = Unit_Base.TeamSize[0];
			float t2 = Unit_Base.TeamSize[1];
			float t3 = Unit_Base.TeamSize[2];
			float totalPop = t1+t2+t3;
			float t1Percent = Mathf.Floor(t1/totalPop*100);
			if(t1Percent>99.9f)
			{
				SceneManager.LoadScene("Score");
			}
		}
	}

	void Update()
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
	public static bool IsDayLight()
	{
		return DayLight.eulerAngles.x>0-10&&DayLight.eulerAngles.x<180+10;
	}

	public void StartNewGame()
	{
		//SceneManager.LoadScene("Main");
		//levelGen.Generate();
	}
}
