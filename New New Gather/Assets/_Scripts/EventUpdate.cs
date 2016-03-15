using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EventUpdate : MonoBehaviour 
{
	[SerializeField] Text scoreText;

	void OnEnable()
	{
		UnityEventManager.StartListeningInt("UpdateFood", SetFood);
		scoreText.text =  "Food: " + 0;
	}
	void OnDisable()
	{
		UnityEventManager.StopListeningInt("UpdateFood", SetFood);
	}
	void SetFood(int amount)
	{

		scoreText.text = "Food: "+ amount;
	}

}
