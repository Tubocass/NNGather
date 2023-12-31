using UnityEngine;
using Gather.UI;

namespace gather
{
    [System.Serializable]
    struct TeamSelect
    {
        public int id;
        public bool isPlayer;
        public ColorOption colorOption;
    }

    [System.Serializable]
    struct ColorOption
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
        //[SerializeField] TeamConfig[] teams;
        //Blackboard globalContext = new Blackboard();
        [SerializeField] TeamSelect[] teamSelections;
        CameraController cameraController;
        [SerializeField] UIController uiController;
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

        }

        void SetupPlayer(TeamSelect selection, Vector2 start)
        {
            TeamConfig teamConfig = ScriptableObject.CreateInstance<TeamConfig>();
            teamConfig.Team = selection.id;
            teamConfig.TeamColor = selection.colorOption.color;
            
            PlayerQueen player = Instantiate(playerPrefab, start, Quaternion.identity)
                .GetComponent<PlayerQueen>();
            
            player.SetTeam(teamConfig);
            input.SetPlayer(player);
            cameraController.SetTarget(player.transform);
            uiController.Setup(player, teamConfig);

        }

        void SetupBot(TeamSelect selection, Vector2 start)
        {
            Queen bot = Instantiate(queenPrefab, start, Quaternion.identity)
                .GetComponent<Queen>();
            TeamConfig teamConfig = ScriptableObject.CreateInstance<TeamConfig>();
            teamConfig.Team = selection.id;
            teamConfig.TeamColor = selection.colorOption.color;
            bot.SetTeam(teamConfig);
        }
    }
}
