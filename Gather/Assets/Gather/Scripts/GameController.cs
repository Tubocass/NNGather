using UnityEngine;
using UnityEngine.UI;
using Gather.AI;
using Gather.UI;


namespace gather
{
    public class GameController : MonoBehaviour
    {
        CameraController cameraController;
        [SerializeField] Queen playerQueen; //  will eventually be spawned after world setup
        [SerializeField] Queen[] otherQueens;
        //Blackboard globalContext = new Blackboard();
        [SerializeField] UIController uiController;
        [SerializeField] int numTeams;
        [SerializeField] TeamConfig[] teams;

        private void Awake()
        {
            cameraController = Camera.main.GetComponent<CameraController>();
        }

        public void StartGame()
        {
            cameraController.SetTarget(playerQueen.transform);
            playerQueen.SetTeam(teams[0]);
            uiController.Setup(playerQueen, teams[0]);

            for(int q = 0; q<otherQueens.Length; q++)
            {
                otherQueens[q].SetTeam(teams[q+1]);
            }
        }

        private void Start()
        {
            StartGame();
        }
    }
}
