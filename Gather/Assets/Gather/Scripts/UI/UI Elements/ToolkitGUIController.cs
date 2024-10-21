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
        PopulationBarController populationBar;
        //PauseMenu pauseMenu;


        private void OnEnable()
        {
            populationBar = GetComponent<PopulationBarController>();
        }

        public override void SetupPlayerUI(Queen playerQueen)
        {
            TeamConfig playerTeam = playerQueen.TeamConfig;
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            Label foodCount = root.Q<Label>(name: "foodCount");
            Label farmerCount = root.Q<Label>(name: "farmerCount");
            Label fighterCount = root.Q<Label>(name: "fighterCount");
            VisualElement farmerImage = root.Q(name: "farmerImage");
            VisualElement fighterImage = root.Q(name: "fighterImage");
            spawnFarmer = root.Q<Button>(name: "farmerButton");
            spawnFighter = root.Q<Button>(name: "fighterButton");
            
            spawnFarmer.clicked += playerQueen.SpawnFarmer;
            spawnFarmer.style.unityBackgroundImageTintColor = playerQueen.TeamConfig.TeamColor;
            spawnFighter.clicked += playerQueen.SpawnFighter;
            spawnFighter.style.unityBackgroundImageTintColor = playerQueen.TeamConfig.TeamColor;
            farmerImage.style.unityBackgroundImageTintColor = playerQueen.TeamConfig.TeamColor;
            fighterImage.style.unityBackgroundImageTintColor = playerQueen.TeamConfig.TeamColor;
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
