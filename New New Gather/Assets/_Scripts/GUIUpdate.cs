using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GUIUpdate : MonoBehaviour 
{
	[SerializeField] Text scoreText, healthText, statText;
	MainMomController mainMoMControl;


	void OnEnable()
	{
		UnityEventManager.StartListening("UpdateFood", SetFood);
		UnityEventManager.StartListening("MainMomChange", MoMChanged);
		UnityEventManager.StartListening("UpdateHealth", SetHealth);
		StartCoroutine(UpdateInfo());
//		healthText.text =  "Health: " + 0;
//		scoreText.text =  "Food: " + 0;
	}
	void OnDisable()
	{
		UnityEventManager.StopListening("UpdateFood", SetFood);
		UnityEventManager.StopListening("UpdateHealth", SetHealth);
		UnityEventManager.StopListening("MainMomChange", MoMChanged);
	}
	void MoMChanged(GameObject Main)
	{
		mainMoMControl = Main.GetComponent<MainMomController>();

	}
	void SetFood(int amount)
	{
		scoreText.text = "Food: "+ amount;
	}
	void SetHealth(int amount)
	{
		healthText.text = "Health: "+ amount;
	}
	void SetStatus(int amount)
	{
		healthText.text = "Health: "+ amount;
	}
	IEnumerator UpdateInfo()
	{
		while(true)
		{
			float totalPop;
			float t1 = MoMController.GetTeamSize(0);
			float t2 = MoMController.GetTeamSize(1);
			float t3 = MoMController.GetTeamSize(2);
			totalPop = t1+t2+t3;

			if(mainMoMControl!= null)
			{
				statText.text = " Farmers: "+ MainMomController.MainMoM.farmers+ " Fighters: "+ MainMomController.MainMoM.fighters
					+ "\n Team 1: "+ t1 +" - "+ Mathf.Floor(t1/totalPop*100) + "%" +  "\n Team 2: "+ t2 +" - "+ Mathf.Floor(t2/totalPop*100) +"%" +"\n Team 3: "+ t3 +" - "+ Mathf.Floor(t3/totalPop*100) +"%";
			}
			yield return new WaitForSeconds(1f);
		}

	}
}
