using UnityEngine;
using UnityEngine.UI;
using Gather.AI;
using Gather.UI;


namespace gather
{
    public class GameController : MonoBehaviour
    {
        CameraController cameraController;
        [SerializeField] Button farmerButton; //  temporary
        [SerializeField] Button fighterButton; //  temporary
        [SerializeField] Queen playerQueen; //  will eventually be spawned after world setup
        Blackboard globalContext = new Blackboard();
        [SerializeField] UIController uiController;
        [SerializeField] int numTeams;
        TeamConfig[] teams;

        private void Awake()
        {
            cameraController = Camera.main.GetComponent<CameraController>();
        }

        public void StartGame()
        {

            cameraController.SetTarget(playerQueen.transform);
            uiController.Setup(playerQueen, teams[0]);
        }

        private void Start()
        {
           
        }
    }
}
