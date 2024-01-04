using UnityEngine;
using System.Collections.Generic;
using Gather.UI;

namespace gather
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] GameObject queenPrefab;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] UIController uiController;
        [SerializeField] ColorOptions colorOptions;
        List<TeamConfig> teams;
        TeamSelect[] teamSelections;
        CameraController cameraController;
        InputManager input;
        LevelSetup levelSetup;

        private void Awake()
        {
            //DontDestroyOnLoad(gameObject);
            cameraController = Camera.main.GetComponent<CameraController>();
            input = GetComponent<InputManager>();
            levelSetup = GetComponent<LevelSetup>();
        }

        private void Start()
        {
            StartGame();
        }

        public void FindTeamSelections()
        {
            int teams = PlayerPrefs.GetInt("teamCount");
            teamSelections = new TeamSelect[teams];
            for(int i = 0; i < teams; i++)
            {
                teamSelections[i] = JsonUtility.FromJson<TeamSelect>(PlayerPrefs.GetString("team"+i));
            }
        }

        public void StartGame()
        {
            // levelSetup.Generate();
            FindTeamSelections();
            teams = new List<TeamConfig>();
            uiController.gameObject.SetActive(true);

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

        TeamConfig NewTeam(TeamSelect selection)
        {
            TeamConfig teamConfig = ScriptableObject.CreateInstance<TeamConfig>();
            teamConfig.Team = selection.id;
            teamConfig.TeamColor = colorOptions.colors[selection.colorOption];
            teams.Add(teamConfig);
            
            return teamConfig;
        }

        void SetupPlayer(TeamSelect selection, Vector2 start)
        {
            TeamConfig teamConfig = NewTeam(selection);
            
            PlayerQueen player = Instantiate(playerPrefab, start, Quaternion.identity)
                .GetComponent<PlayerQueen>();
            
            player.SetTeam(teamConfig);
            input.SetPlayer(player);
            cameraController.SetTarget(player.transform);
            uiController.SetupPlayerUI(player, teamConfig);
        }

        void SetupBot(TeamSelect selection, Vector2 start)
        {
            TeamConfig teamConfig = NewTeam(selection);

            Queen bot = Instantiate(queenPrefab, start, Quaternion.identity)
                .GetComponent<Queen>();
            bot.SetTeam(teamConfig);
        }
    }
}
