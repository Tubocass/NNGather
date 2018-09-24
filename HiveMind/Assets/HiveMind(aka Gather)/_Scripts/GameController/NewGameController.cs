using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;
public class NewGameController : MonoBehaviour
{
    private static NewGameController gameControl;
    public static NewGameController Instance
    {
        get
        {
            if (!gameControl)
            {
                gameControl = FindObjectOfType(typeof(NewGameController)) as NewGameController;
                if (!gameControl)
                {
                    Debug.LogError("There needs to be one active GameController script on a GameObject in your scene.");
                }
            }
            return gameControl;
        }
    }
    private NewLevelGenerator levelGen = new NewLevelGenerator();
    public List<int> TeamSize = new List<int>();
    List<MoMController> Bots = new List<MoMController>();
    public int NumberOfTeams {
        get
        {
            return numPlayers;
        }
    } //readonly
    int numPlayers = 0;
    PlayerMomController[] Players;

    // GUIController
    // Object pools for units

    private string gameDataProjectFilePath = "/StreamingAssets/data.json";
    LevelProperties levelProps;

    private void Start()
    {
        //levelGen.Init();
        Players = GameObject.FindObjectsOfType<PlayerMomController>();
        numPlayers = Players.Length + Bots.Count;
        for (int t = 0; t < numPlayers; t++)
        {
            TeamSize.Add(0);
        }
    }
    private void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            levelProps = JsonUtility.FromJson<LevelProperties>(dataAsJson);
            Debug.Log("File Loaded");
            //seed = levelProps.seed;
            //useRandomSeed = levelProps.useRandomSeed;
            //map = levelProps.map;
        }
        else
        {
            levelProps = new LevelProperties();
        }
    }

    public float TeamSizePercent(int t)
    {
        if (TeamSize.Count > 0)
        {
            float totalPop = 0;
            for (int i = 0; i < numPlayers; i++)
            {
                totalPop += TeamSize[i];
            }
            return TeamSize[t] / totalPop * 100;
        }
        else return 0f;
    }

}
