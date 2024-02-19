using gather;
using Gather.AI.FSM.States;
using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.Controllers
{
    public class FarmerFSM_Controller : FSM_Controller
    {
        FarmerDrone drone;
        EnemyDetector enemyDetector;

        protected override void Init()
        {
            drone = GetComponent<FarmerDrone>();
            enemyDetector = GetComponent<EnemyDetector>();

            DroneState_Search searchState = new DroneState_Search(drone);
            DroneState_Engage engageState = new DroneState_Engage(drone);
            DroneState_Flee fleeState = new DroneState_Flee(drone);
            DroneState_Return returnState = new DroneState_Return(drone);
            initialState = searchState;

            To_DroneState_Search toSearch = new(drone, searchState);
            To_DroneState_Engage toEngage = new(drone, engageState);
            To_DroneState_Flee toFlee = new(drone, fleeState);
            To_DroneState_Return toReturn = new(drone, returnState);

            searchState.AddTransitions(toFlee, toEngage, toReturn);
            engageState.AddTransitions(toFlee, toReturn, toSearch);
            fleeState.AddTransitions(toSearch, toReturn);
            returnState.AddTransitions(toFlee, toSearch);
        }

        public override void Tick()
        {
            enemyDetector.Detect();
            base.Tick();
        }
    }
}