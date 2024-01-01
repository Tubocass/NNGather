using UnityEngine;
using System.Collections.Generic;
using Gather.UI;

namespace gather
{
    [System.Serializable]
    public struct TeamSelect
    {
        public int id;
        public bool isPlayer;
        public ColorOption colorOption;
    }

    [System.Serializable]
    public struct ColorOption
    {
        public Color color;
        public bool isSelected;
    }

    public class GameController : MonoBehaviour
    {
       /*
        *  Take Team Selections (isPlayer, Color)
        *  for bots, setup prefabs and teams
        *  for players, setup camera and controls in addition to teams
        *  
       */
        [SerializeField] GameObject queenPrefab;
        [SerializeField] GameObject playerPrefab;
        //[SerializeField] Queen playerQueen;
        //[SerializeField] Queen[] bots;
        [SerializeField] List<TeamConfig> teams;
        //Blackboard globalContext = new Blackboard();
        [SerializeField] TeamSelect[] teamSelections;
        CameraController cameraController;
        [SerializeField] UIController uiController;
        [SerializeField] PopulationBar populationBar;
        InputManager input;
        LevelSetup levelSetup;

        private void Awake()
        {
            cameraController = Camera.main.GetComponent<CameraController>();
            input = GetComponent<InputManager>();
            levelSetup = GetComponent<LevelSetup>();
        }
        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            // levelSetup.Generate();
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

            populationBar.SetTeams(teams.ToArray());
        }

        TeamConfig NewTeam(TeamSelect selection)
        {
            TeamConfig teamConfig = ScriptableObject.CreateInstance<TeamConfig>();
            teamConfig.Team = selection.id;
            teamConfig.TeamColor = selection.colorOption.color;
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
            uiController.Setup(player, teamConfig);
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
