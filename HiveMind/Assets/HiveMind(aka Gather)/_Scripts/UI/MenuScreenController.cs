using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MenuScreenController : MonoBehaviour 
{
	void Start()
	{
		//NetworkLobbyManager.singleton.GetComponent<Canvas>().enabled = false;
		//GameController.instance.gameObject.SetActive(true);
	}
	public void StartGame()
	{
		SceneManager.LoadScene("Game Setup");
	}
}
