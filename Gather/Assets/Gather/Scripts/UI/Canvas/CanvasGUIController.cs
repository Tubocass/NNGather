using UnityEngine;
using UnityEngine.UI;

namespace Gather.UI.Canvas
{
    public class CanvasGUIController : GUIController
    {
        [SerializeField] PauseMenu pauseMenu;
        [SerializeField] PopulationBar populationBar;
        [SerializeField] ScoreDisplay foodDisplay;
        [SerializeField] ScoreDisplay farmerDisplay;
        [SerializeField] ScoreDisplay fighterDisplay;
        [SerializeField] HealthBar healthBar;
        [SerializeField] Button spawnFarmer;
        [SerializeField] Button spawnFighter;
        [SerializeField] Button farmerAnchor;
        [SerializeField] Button fighterAnchor;

        public override void SetupPlayerUI(Queen playerQueen)
        {
            TeamConfig playerTeam = playerQueen.TeamConfig;
            foodDisplay.SetCounter(playerQueen.GetFoodCounter());
            farmerDisplay.SetCounter(playerTeam.UnitManager.GetUnitCounter(UnitType.Farmer));
            fighterDisplay.SetCounter(playerTeam.UnitManager.GetUnitCounter(UnitType.Fighter));
            healthBar.SetCounter(playerQueen.Health.GetCounter());

            spawnFarmer.onClick.AddListener(playerQueen.SpawnFarmer);
            spawnFighter.onClick.AddListener(playerQueen.SpawnFighter);
            //farmerAnchor.onClick.AddListener(playerQueen.PlaceFoodAnchor);
            //fighterAnchor.onClick.AddListener(playerQueen.PlaceFightAnchor);
        }

        public override void SetupPopulationBar(TeamConfig[] teams) 
        {
            //populationBar.gameObject.SetActive(true);
            populationBar.SetTeams(teams);

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.TogglePauseMenu();
            }
        }
    }
}