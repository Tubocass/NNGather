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
            Blackboard context = drone.Blackboard;

            State_Search searchState = new State_Search(drone, context);
            State_Engage engageState = new State_Engage(drone, context);
            State_Flee fleeState = new State_Flee(drone, context);
            State_Return returnState = new State_Return(drone, drone.GetMyQueen().transform);
            initialState = searchState;

            ToStateSearch toSearch = new(drone, searchState);
            ToStateEngage toEngage = new(drone, engageState);
            ToStateFlee toFlee = new(drone, fleeState);
            ToStateReturn toReturn = new(drone, returnState);

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