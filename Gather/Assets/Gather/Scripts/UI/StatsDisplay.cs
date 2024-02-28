using UnityEngine;
using UnityEngine.UI;

namespace Gather.UI
{
    public class StatsDisplay : MonoBehaviour
    {
        [SerializeField] ScoreDisplay foodDisplay;
        [SerializeField] ScoreDisplay farmerDisplay;
        [SerializeField] ScoreDisplay fighterDisplay;
        [SerializeField] HealthBar healthBar;
        [SerializeField] Button spawnFarmer;
        [SerializeField] Button spawnFighter;
        [SerializeField] Button farmerAnchor;
        [SerializeField] Button fighterAnchor;

        public void SetupPlayerUI(Queen playerQueen, TeamConfig playerTeam)
        {
            foodDisplay.SetCounter(playerQueen.GetFoodCounter());
            farmerDisplay.SetCounter(playerTeam.UnitManager.GetUnitCounter(UnitType.Farmer));
            fighterDisplay.SetCounter(playerTeam.UnitManager.GetUnitCounter(UnitType.Fighter));
            healthBar.SetCounter(playerQueen.Health.GetCounter());

            spawnFarmer.onClick.AddListener(playerQueen.SpawnFarmer);
            spawnFighter.onClick.AddListener(playerQueen.SpawnFighter);
            farmerAnchor.onClick.AddListener(playerQueen.PlaceFoodAnchor);
            fighterAnchor.onClick.AddListener(playerQueen.PlaceFightAnchor);
        }
    }
}