using gather;

namespace Gather.AI
{
    public class FighterFSM_Controller : FSM_Controller
    {
        State_Hunt huntState;
        State_Engage engageState;

        protected override void Init()
        {
            FighterDrone drone = GetComponent<FighterDrone>();
            Blackboard context = drone.GetBlackboard();

            huntState = new State_Hunt(drone, context);
            engageState = new State_Engage(drone, context);
            initialState = huntState;

            huntState.transistions.Add(
                new ToStateEngage(drone, engageState)
                );
        }
    }
}