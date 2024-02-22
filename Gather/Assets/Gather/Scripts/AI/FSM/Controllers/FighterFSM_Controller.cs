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
            Blackboard bb = drone.Blackboard;

            DroneState_Hunt huntState = new DroneState_Hunt(bb);
            DroneState_Engage engageState = new DroneState_Engage(bb);
            initialState = huntState;

            To_DroneState_Engage toEngage = new To_DroneState_Engage(bb, engageState);
            To_DroneState_Hunt toHunt = new To_DroneState_Hunt(bb, huntState);

            huntState.AddTransitions(toEngage);
            engageState.AddTransitions(toHunt);
        }
    }
}