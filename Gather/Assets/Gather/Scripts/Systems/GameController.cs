using UnityEngine;
using System.Collections.Generic;
using Gather.UI;
using UnityEditor;

namespace Gather
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] GameObject queenPrefab;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] GameObject sarlacPrefab;
        [SerializeField] GUIController uiController;
        [SerializeField] ColorOptions colorOptions;
        [SerializeField] LevelSetup levelSetup;

        List<TeamConfig> teams;
        TeamSelect[] teamSelections;
        CameraController cameraController;
        InputManager input;

        private void Awake()
        {
            //DontDestroyOnLoad(gameObject);
            cameraController = Camera.main.GetComponent<CameraController>();
            input = GetComponent<InputManager>();
        }

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            levelSetup.Generate();
            FindTeamSelections();
            teams = new List<TeamConfig>();
            uiController.gameObject.SetActive(true);

            SetupSarlac();

            for (int t = 0; t < teamSelections.Length; t++)
            {
                if (teamSelections[t].isPlayer)
                {
                    SetupPlayer(teamSelections[t], levelSetup.GetStartLocation());
                } else
                {
                    SetupBot(teamSelections[t], levelSetup.GetStartLocation());
                }
            }

            uiController.SetupPopulationBar(teams.ToArray());
        }

        public void FindTeamSelections()
        {
            int teams = PlayerPrefs.GetInt("teamCount");
            teamSelections = new TeamSelect[teams];
            for (int i = 0; i < teams; i++)
            {
                teamSelections[i] = JsonUtility.FromJson<TeamSelect>(PlayerPrefs.GetString("team" + i));
            }
        }

        void SetupSarlac()
        {
            TeamSelect envTeam = new TeamSelect(-1, false, 0);
            TeamConfig teamConfig = NewTeam(envTeam);
            teams.Remove(teamConfig);
            
            FoodBush[] bushes = FindObjectsByType<FoodBush>(FindObjectsSortMode.InstanceID);
            int index = Random.Range(0, bushes.Length);

            Sarlac sarlac = Instantiate(sarlacPrefab, bushes[index].transform.position, Quaternion.identity).GetComponent<Sarlac>();
            sarlac.SetTeam(teamConfig);
            sarlac.SetHome(bushes[index].transform);
        }

        void SetupPlayer(TeamSelect selection, Vector2 start)
        {
            TeamConfig teamConfig = NewTeam(selection);
            
            Queen player = Instantiate(playerPrefab, start, Quaternion.identity)
                .GetComponent<Queen>();
            
            player.SetTeam(teamConfig);
            input.SetPlayer(player);
            cameraController.SetTarget(player.transform);
            uiController.SetupPlayerUI(player);
        }

        void SetupBot(TeamSelect selection, Vector2 start)
        {
            TeamConfig teamConfig = NewTeam(selection);

            Queen bot = Instantiate(queenPrefab, start, Quaternion.identity)
                .GetComponent<Queen>();
            bot.SetTeam(teamConfig);
        }

        TeamConfig NewTeam(TeamSelect selection)
        {
            TeamConfig teamConfig = ScriptableObject.CreateInstance<TeamConfig>();
            teamConfig.TeamID = selection.id;
            teamConfig.TeamColor = colorOptions.colors[selection.colorOption];
            teams.Add(teamConfig);

            return teamConfig;
        }
    }
}
