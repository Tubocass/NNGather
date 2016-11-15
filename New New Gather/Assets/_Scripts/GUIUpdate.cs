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
			float totalPop, t1, t2, t3;
			if(mainMoMControl!= null)
			{
				statText.text = "Units: "+ mainMoMControl.GetTeamSize();
			}
			yield return new WaitForSeconds(1f);
		}

	}
}
