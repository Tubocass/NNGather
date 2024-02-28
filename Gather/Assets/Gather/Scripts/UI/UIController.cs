using UnityEngine;

namespace Gather.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] PauseMenu pauseMenu;
        [SerializeField] StatsDisplay statsDisplay;
        [SerializeField] PopulationBar populationBar;
        //Blackboard globalContext;

        public void SetupPlayerUI(Queen playerQueen, TeamConfig playerTeam)
        {
            statsDisplay.SetupPlayerUI(playerQueen, playerTeam);
        }

        public void SetupPopulationBar(TeamConfig[] teams) 
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