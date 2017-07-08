using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MenuManager : MonoBehaviour 
{
/*
	Responsibilities:
	Handle Scene Transition
	Handle user-modified Levelgen variables
	Call GameController.StartNewGame
*/
	public void SPGameSetupScene()
	{
		SceneManager.LoadScene("Game Setup");
	}
	public void MPGameSetupScene()
	{
		SceneManager.LoadScene("Lobby");
	}
}
