using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SuperManager : NetworkBehaviour 
{
	public IUIState UIState 
	{
		get{ return uiState; }
		set 
		{
			if (uiState != null)
			{
				uiState.ExitState ();
			}
			uiState = value;
			uiState.EnterState ();
		}
	}

	public enum GameState{OutOfGame, InGame};
	public GameState gameState;
	private IUIState uiState;
	LevelSettings level;
	GameSetupManager setupScreen;
	GUIController inGameGUI;
/*
	Responsibilities:
	Handle Scene Transition between GameInstance and MainMenus
	Handle user-modified Levelgen variables
	Create GameInstance with Variables
*/
	public void CreateGameInstance()
	{
	}
	public void ToGameSetupScreen()
	{
		setupScreen.gameObject.SetActive(true);
	}
	public void ToOpeningMenuScreen()
	{
	}
	public void ToScoreScreen()
	{
	}
}
