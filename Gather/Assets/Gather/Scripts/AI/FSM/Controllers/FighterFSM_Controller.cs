using gather;
using Gather.AI.FSM.States;
using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.Controllers
{
    public class FighterFSM_Controller : FSM_Controller
    {
        protected override void Init()
        {
            FighterDrone drone = GetComponent<FighterDrone>();

            State_Hunt huntState = new State_Hunt(drone);
            State_Engage engageState = new State_Engage(drone);
            initialState = huntState;

            ToStateEngage toEngage = new ToStateEngage(drone, engageState);
            ToStateHunt toHunt = new ToStateHunt(drone, huntState);

            huntState.AddTransitions(toEngage);
            engageState.AddTransitions(toHunt);
        }
    }
}