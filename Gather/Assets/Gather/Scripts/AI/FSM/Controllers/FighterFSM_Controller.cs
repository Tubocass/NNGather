using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Controllers
{
    public class FighterFSM_Controller : FSM_Controller
    {
        protected override void Init()
        {
            FighterDrone drone = GetComponent<FighterDrone>();
            Blackboard bb = drone.Blackboard;

            DroneStateFactory factory = new DroneStateFactory(bb);

            factory.DroneState_Hunt.AddTransitions(factory.ToEngage);
            factory.DroneState_Engage.AddTransitions(factory.To_Hunt);

            initialState = factory.DroneState_Hunt;
        }
    }
}