using gather;
using UnityEngine;
using UnityEngine.UI;

namespace Gather.UI
{
    public class StatsDisplay : MonoBehaviour
    {
        [SerializeField] ScoreDisplay foodDisplay;
        [SerializeField] ScoreDisplay farmerDisplay;
        [SerializeField] ScoreDisplay fighterDisplay;
        [SerializeField] Button spawnFarmer;
        [SerializeField] Button spawnFighter;
        [SerializeField] Button farmerAnchor;
        [SerializeField] Button fighterAnchor;

        public void SetupPlayerUI(Queen playerQueen, TeamConfig playerTeam)
        {
            farmerDisplay.SetCounter(playerTeam.GetUnitCounter(UnitType.Farmer));
            fighterDisplay.SetCounter(playerTeam.GetUnitCounter(UnitType.Fighter));
         
            foodDisplay.SetCounter(playerQueen.GetFoodCounter());
            spawnFarmer.onClick.AddListener(playerQueen.SpawnFarmer);
            spawnFighter.onClick.AddListener(playerQueen.SpawnFighter);
            farmerAnchor.onClick.AddListener(playerQueen.PlaceFoodAnchor);
            fighterAnchor.onClick.AddListener(playerQueen.PlaceFightAnchor);
        }
    }
}