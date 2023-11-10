using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Gather.AI;
using gather;


namespace Gather.UI
{
    public class UIController : MonoBehaviour
    {
        //[SerializeField] TeamConfig team;
        //[SerializeField] Queen playerQueen;

        [SerializeField] ScoreDisplay foodDisplay;
        [SerializeField] ScoreDisplay farmerDisplay;
        [SerializeField] ScoreDisplay fighterDisplay;
        [SerializeField] Button farmerButton;
        [SerializeField] Button fighterButton;
        Blackboard globalContext;

        private void Start()
        {
       
        }

        public void Setup(Queen playerQueen, TeamConfig playerTeam)
        {
            if (playerTeam)
            {
                farmerDisplay.count = playerTeam.GetUnitCounter(TeamConfig.UnitType.Farmer);
                fighterDisplay.count = playerTeam.GetUnitCounter(TeamConfig.UnitType.Fighter);
            }
            if (playerQueen)
            {
                foodDisplay.count = playerQueen.foodCounter;
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