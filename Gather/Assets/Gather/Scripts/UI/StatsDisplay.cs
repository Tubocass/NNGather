using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Gather.UI
{
    public class StatsDisplay : MonoBehaviour
    {
        [SerializeField] ScoreDisplay foodDisplay;
        [SerializeField] ScoreDisplay farmerDisplay;
        [SerializeField] ScoreDisplay fighterDisplay;
        [SerializeField] HealthBar healthBar;
        [SerializeField] UIDocument document;
        //[SerializeField] Button spawnFarmer;
        //[SerializeField] Button spawnFighter;
        //[SerializeField] Button farmerAnchor;
        //[SerializeField] Button fighterAnchor;
        DisplayNumber foodLabel;


        public void SetupPlayerUI(Queen playerQueen, TeamConfig playerTeam)
        {
            foodDisplay.SetCounter(playerQueen.GetFoodCounter());
            farmerDisplay.SetCounter(playerTeam.UnitManager.GetUnitCounter(UnitType.Farmer));
            fighterDisplay.SetCounter(playerTeam.UnitManager.GetUnitCounter(UnitType.Fighter));
            healthBar.SetCounter(playerQueen.Health.GetCounter());

            VisualElement root = document.rootVisualElement;
            Label foodCount = root.Q<Label>(name: "foodCount");
            foodLabel = new DisplayNumber(foodCount, playerQueen.GetFoodCounter());
            //spawnFarmer.onClick.AddListener(playerQueen.SpawnFarmer);
            //spawnFighter.onClick.AddListener(playerQueen.SpawnFighter);
            //farmerAnchor.onClick.AddListener(playerQueen.PlaceFoodAnchor);
            //fighterAnchor.onClick.AddListener(playerQueen.PlaceFightAnchor);
        }
    }
}