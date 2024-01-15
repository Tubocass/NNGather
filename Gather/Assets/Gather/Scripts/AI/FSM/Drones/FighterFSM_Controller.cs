using gather;

namespace Gather.AI
{
    public class FighterFSM_Controller : FSM_Controller
    {
        protected override void Init()
        {
            FighterDrone drone = GetComponent<FighterDrone>();
            Blackboard context = drone.GetBlackboard();

            State_Hunt huntState = new State_Hunt(drone, context);
            State_Engage engageState = new State_Engage(drone, context);
            initialState = huntState;

            ToStateEngage toEngage = new ToStateEngage(drone, engageState);
            ToStateHunt toHunt = new ToStateHunt(drone, huntState);

            huntState.AddTransitions(toEngage);
            engageState.AddTransitions(toHunt);
        }
    }
}