using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

[System.Serializable]
public struct PlayerSelection
{
    public bool isHuman;
    public string playerHandle;
    public int teamNumber;
    public Color teamColor;

    public PlayerSelection(bool human, string tag, int team, Color color)
    {
        isHuman = human;
        playerHandle = tag;
        teamNumber = team;
        teamColor = color;
    }
}
public class GameSetupManager : MonoBehaviour 
{
    //[SerializeField]Slider[] sliders;
    //private NetworkManager nm;
    [SerializeField] string seed;
    int bots, humans, sarlacs, foodScarcity;
    LevelProperties lvlProps;
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";

    [SerializeField] Color[] teamColors = new Color[5];
    bool[] availableColors;
    int[] colorIndexes;
 
    [SerializeField] int maxPlayers = 4;
    TeamSelection[] teamSelections;
    bool[] activePlayers;
    [SerializeField] GameObject TeamSelectPrefab;
    [SerializeField] Transform teamSelectRow;

    int[,] mapSize;
    bool useRandomSeed;

    private void Start()
    {
        availableColors = new bool[teamColors.Length];
        activePlayers = new bool[maxPlayers];
        colorIndexes = new int[maxPlayers];

        for (int c = 0; c < availableColors.Length; c++)
        {
            availableColors[c] = true;
        }

        teamSelections = new TeamSelection[maxPlayers];
        PlayerSelection[] players = new PlayerSelection[maxPlayers];

        for(int t = 0; t<maxPlayers; t++)
        {
            colorIndexes[t] = -1;
            activePlayers[t] = true;
            if (t==0)
            {
                players[t] = new PlayerSelection(true, "Human", t, SelectColor(t));
            }
            else
            {
                players[t] = new PlayerSelection(false, "AI", t, SelectColor(t));
            }
            colorIndexes[t] = t;
            GameObject teamSelectItem = Instantiate(TeamSelectPrefab, teamSelectRow);
            teamSelections[t] = teamSelectItem.GetComponent<TeamSelection>();
            teamSelections[t].manager = this;
            teamSelections[t].Setup(players[t]);
              
        }
    }
    public void OnChangeBotValue(float b)
	{
        bots = (int)b;
	}
	public void OnChangeSarlacValue(float s)
	{
        sarlacs = (int)s;
	}
	public void OnChangePlantValue(float p)
	{
        foodScarcity = (int)p;
	}
    public void OnChangeSeedValue(string s)
    {
        seed = s;
    }
    public void OnChangeRandomSeed(bool b)
    {
        useRandomSeed = b;
    }

    public Color SelectColor(int team)
    {
        int prevIndex = colorIndexes[team];
        for (int c = 0; c < availableColors.Length; c++)
        {
            if (availableColors[c])
            {
                availableColors[c] = false;
                colorIndexes[team] = c;
                if (prevIndex != -1)
                {
                    availableColors[prevIndex] = true;
                }

                return teamColors[c];
            }
        } 
        return Color.white;
    }
    public void ConfirmPlayer(int team,bool isActive)
    {
        activePlayers[team] = isActive;
    }

    public void SaveGameData()
    {
        int numPlayers = 0;
        for(int p = 0;p<maxPlayers;p++)
        {
            if (activePlayers[p])
            {
                numPlayers += 1;
            }
        }
        PlayerSelection[] players = new PlayerSelection[numPlayers];
        for (int p = 0; p < numPlayers; p++)
        {
            for (int ap = 0; ap < maxPlayers; ap++)
            {
                if (activePlayers[ap])
                { 
                    players[p] = teamSelections[ap].AddPlayer();
                    activePlayers[ap] = false;
                    break;
                }
            }
        }

        mapSize = new int[64, 64];
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }
 
        lvlProps = new LevelProperties(seed, useRandomSeed, bots, sarlacs, players, mapSize);
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
