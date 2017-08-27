using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameSetupManager : MonoBehaviour 
{
	[SerializeField]Slider[] sliders;
	int[] settings = {3,8,4};//bots,sarlac,plants
	public NetworkManager nm;

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
		nm.StartHost();
		settings =  new int[]{(int)sliders[0].value, (int)sliders[1].value, (int)sliders[2].value};
		GameController.instance.CompleteSetup(settings);

	}
}
