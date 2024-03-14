using UnityEngine;
using UnityEngine.UIElements;

namespace Gather.UI {
    public class ToolkitGUIController : GUIController
    {
        DisplayNumber foodCounter;
        DisplayNumber farmerCounter;
        DisplayNumber fighterCounter;
        Button spawnFarmer;
        Button spawnFighter;
        //HealthBar healthBar;
        //Button farmerAnchor;
        //Button fighterAnchor;
        PopulationBarElement populationBar;
        //PauseMenu pauseMenu;


        private void OnEnable()
        {
            populationBar = GetComponent<PopulationBarElement>();
        }

        public override void SetupPlayerUI(Queen playerQueen)
        {
            TeamConfig playerTeam = playerQueen.TeamConfig;
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            Label foodCount = root.Q<Label>(name: "foodCount");
            Label farmerCount = root.Q<Label>(name: "farmerCount");
            Label fighterCount = root.Q<Label>(name: "fighterCount");
            spawnFarmer = root.Q<Button>(name: "farmerButton");
            spawnFighter = root.Q<Button>(name: "fighterButton");
            
            spawnFarmer.clicked += playerQueen.SpawnFarmer;
            spawnFighter.clicked += playerQueen.SpawnFighter;
            foodCounter = new DisplayNumber(foodCount, playerQueen.GetFoodCounter());
            farmerCounter = new DisplayNumber(farmerCount, playerTeam.UnitManager.GetUnitCounter(UnitType.Farmer));
            fighterCounter = new DisplayNumber(fighterCount, playerTeam.UnitManager.GetUnitCounter(UnitType.Fighter));
        }

        public override void SetupPopulationBar(TeamConfig[] teams)
        {
            populationBar.SetTeams(teams);
        }

        private void Update()
        {
            //populationBar.DrawBar();
        }
    }
}
