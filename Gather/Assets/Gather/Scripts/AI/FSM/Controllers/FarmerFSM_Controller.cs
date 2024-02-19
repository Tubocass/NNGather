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

            State_Search searchState = new State_Search(drone);
            State_Engage engageState = new State_Engage(drone);
            State_Flee fleeState = new State_Flee(drone);
            State_Return returnState = new State_Return(drone);
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