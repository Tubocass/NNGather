﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.IO;
public class NewGameController : NetworkBehaviour
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
    private NewLevelGenerator levelGen;
    public List<int> TeamSize = new List<int>();
    //List<MoMController> Bots = new List<MoMController>();
    public int NumberOfTeams {
        get
        {
            return numPlayers;
        }
    } //readonly
    public bool IsDaylight = true;
    int numPlayers = 0;
    [SerializeField] GameObject camFab, guiFab;

    // GUI Setup
    // Object pools for units
    // IsGameOver

    private string gameDataProjectFilePath = "/StreamingAssets/data.json";
    LevelProperties levelProps;

    public override void OnStartServer()
    {
        base.OnStartServer();
        LoadGameData();
        levelGen = GetComponent<NewLevelGenerator>();
        numPlayers = levelProps.players.Length;

        for (int t = 0; t < numPlayers; t++)
        {
            TeamSize.Add(0);
        }

        levelGen.SpawnMoMs(levelProps.players);
    }

    private void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            levelProps = JsonUtility.FromJson<LevelProperties>(dataAsJson);
            Debug.Log("File Loaded");
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
