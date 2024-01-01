using UnityEngine;
using UnityEngine.UI;
using Gather.AI;
using gather;

namespace Gather.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] ScoreDisplay foodDisplay;
        [SerializeField] ScoreDisplay farmerDisplay;
        [SerializeField] ScoreDisplay fighterDisplay;
        [SerializeField] Button farmerButton;
        [SerializeField] Button fighterButton;
        Blackboard globalContext;

        public void Setup(Queen playerQueen, TeamConfig playerTeam)
        {
            if (playerTeam)
            {
                farmerDisplay.count = playerTeam.GetUnitCounter(UnitType.Farmer);
                fighterDisplay.count = playerTeam.GetUnitCounter(UnitType.Fighter);
            }
            if (playerQueen)
            {
                foodDisplay.count = playerQueen.GetBlackboard().GetValue<Counter>(Configs.FoodCounter);
                farmerButton.onClick.AddListener(playerQueen.SpawnFarmer);
                fighterButton.onClick.AddListener(playerQueen.SpawnFighter);
            }
        }

        private void Update()
        {
            
        }

        void ToggleMainMenu()
        {
            // state = !state
            // if(state) { Pause(); display(menu); }

        }

        public void NewGameMenu()
        {
            /*  Select numTeams
             *  Select colors (use defaults)
             *  Submit and Start
            */
        }
    }
}