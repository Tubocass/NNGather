using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIUpdate : MonoBehaviour 
{
	[SerializeField] Text scoreText, healthText;

	void OnEnable()
	{
		UnityEventManager.StartListening("UpdateFood", SetFood);
		UnityEventManager.StartListening("UpdateHealth", SetHealth);
//		healthText.text =  "Health: " + 0;
//		scoreText.text =  "Food: " + 0;
	}
	void OnDisable()
	{
		UnityEventManager.StopListening("UpdateFood", SetFood);
		UnityEventManager.StopListening("UpdateHealth", SetHealth);
	}
	void SetFood(int amount)
	{
		scoreText.text = "Food: "+ amount;
	}
	void SetHealth(int amount)
	{
		healthText.text = "Health: "+ amount;
	}

}
