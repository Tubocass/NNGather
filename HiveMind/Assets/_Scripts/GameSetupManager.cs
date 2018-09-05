using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class GameSetupManager : MonoBehaviour 
{
	[SerializeField]Slider[] sliders;
	public NetworkManager nm;
    int bots, sarlacs, foodScarcity;
    LevelProperties lvlProps;
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";
    Color[] teamColors = new Color[5];

    public void OnChangeBotValue(int b)
	{
        bots = b;
	}
	public void OnChangeSarlacValue(int s)
	{
        sarlacs = s;
	}
	public void OnChangePlantValue(int p)
	{
        foodScarcity = p;
	}

    public void SaveGameData()
    {
        lvlProps = new LevelProperties(bots,sarlacs,teamColors);
        string dataAsJson = JsonUtility.ToJson(lvlProps);

        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

    }

    public void StartGame()
	{
		nm.StartHost();

	}
}
