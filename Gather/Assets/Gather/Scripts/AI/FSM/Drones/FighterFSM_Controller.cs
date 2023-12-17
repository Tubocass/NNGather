using gather;

namespace Gather.AI
{
    public class FighterFSM_Controller : FSM_Controller
    {
        State_Hunt huntState;
        State_Engage engageState;

        public FighterFSM_Controller (FighterDrone drone, Blackboard context)
        {
            huntState = new State_Hunt(drone, context);
            engageState = new State_Engage(drone, context);
            initialState = huntState;

            huntState.transistions.Add(
                new ToStateEngage(drone, engageState)
                );
        }
    }
}