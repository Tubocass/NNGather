using System.Collections;
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
    //private NewLevelGenerator levelGen;
    public List<int> TeamSize = new List<int>();
    List<MoMController> Bots = new List<MoMController>();
    public int NumberOfTeams {
        get
        {
            return numPlayers;
        }
    } //readonly
    public bool IsDaylight = true;
    int numPlayers = 0;
    [SerializeField] GameObject camFab, guiFab;
    PlayerMomController[] Players;
    [SerializeField] EnemyMoMController[] enemies;
    protected static List<FarmerController> FarmerPool = new List<FarmerController>();//object pool
    protected static List<FighterController> FighterPool = new List<FighterController>();//object pool
    protected static List<DaughterController> DaughterPool = new List<DaughterController>();//object pool
    protected static List<MoMController> MoMPool = new List<MoMController>();//object pool

    // GUIController
    // Object pools for units

    private string gameDataProjectFilePath = "/StreamingAssets/data.json";
    LevelProperties levelProps;

    public override void OnStartServer()
    {
        base.OnStartServer();
        //levelGen.Init();

        Players = FindObjectsOfType<PlayerMomController>();
        enemies = FindObjectsOfType<EnemyMoMController>();
        numPlayers = Players.Length + enemies.Length;
        for (int t = 0; t < numPlayers; t++)
        {
            TeamSize.Add(0);
        }
        //In a networkgame this should be done on [Client] only 
        /*for (int p = 0; p < Players.Length; p++)
        {
            GameObject mainCam, canvas;
            GUIController GUI;
            mainCam = (GameObject)Instantiate(camFab, Players[p].transform.position + Vector3.up * 30, Quaternion.identity);
            mainCam.transform.Rotate(90, 0, 0);
            Players[p].GetComponent<InputControls>().SetCamera(mainCam.GetComponent<CameraFollow>());
            canvas = (GameObject)Instantiate(guiFab, Vector3.zero, Quaternion.identity);
            GUI = canvas.GetComponent<GUIController>();
            GUI.SetMoM(Players[p]);
        }*/
        
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
