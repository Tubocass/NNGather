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

            DroneState_Hunt huntState = new DroneState_Hunt(drone.Blackboard);
            DroneState_Engage engageState = new DroneState_Engage(drone.Blackboard);
            initialState = huntState;

            To_DroneState_Engage toEngage = new To_DroneState_Engage(drone.Blackboard, engageState);
            To_DroneState_Hunt toHunt = new To_DroneState_Hunt(drone.Blackboard, huntState);

            huntState.AddTransitions(toEngage);
            engageState.AddTransitions(toHunt);
        }
    }
}