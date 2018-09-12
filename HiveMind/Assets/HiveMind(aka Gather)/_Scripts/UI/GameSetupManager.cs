using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class GameSetupManager : MonoBehaviour 
{
    //[SerializeField]Slider[] sliders;
    //private NetworkManager nm;
    [SerializeField] string seed;
    int bots, sarlacs, foodScarcity;
    LevelProperties lvlProps;
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";
    [SerializeField] Color[] teamColors = new Color[5];
    [SerializeField] NewLevelGenerator newGen;
    int[,] mapSize;
    bool useRandomSeed;

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
    public void OnChangeSeedValue(string s)
    {
        seed = s;
    }
    public void OnChangeRandomSeed(bool b)
    {
        useRandomSeed = b;
    }

    public void SaveGameData()
    {
        mapSize = new int[64, 64];
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }
 
        lvlProps = new LevelProperties(seed, useRandomSeed, bots, sarlacs, teamColors, mapSize);
        string dataAsJson = JsonUtility.ToJson(lvlProps);

        string filePath = Application.dataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);
        StartGame();
    }

    public void StartGame()
	{
        //nm.StartHost();
        SceneManager.LoadSceneAsync("LevelGeneration");
       // newGen.Init();
    }
}
