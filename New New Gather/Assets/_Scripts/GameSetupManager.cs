using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameSetupManager : MonoBehaviour 
{
	int[] settings = new int[3];//bots,sarlac,plants
	[SerializeField]Slider[] sliders;

	public void OnChangeBotValue()
	{
		settings[0] = (int)sliders[0].value;
	}
	public void OnChangeSarlacValue()
	{
		settings[1] = (int)sliders[1].value;
	}
	public void OnChangePlantValue()
	{
		settings[2] = (int)sliders[2].value;
	}

	public void StartGame()
	{
		GameController.instance.CompleteSetup(settings);
		SceneManager.LoadScene("Main");
	}
}
