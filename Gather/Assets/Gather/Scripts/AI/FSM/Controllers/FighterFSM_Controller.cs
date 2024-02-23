using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Controllers
{
    public class FighterFSM_Controller : FSM_Controller
    {
        DroneStateFactory factory;

        protected override void Init()
        {
            FighterDrone drone = GetComponent<FighterDrone>();
            Blackboard bb = drone.Blackboard;
            factory = new DroneStateFactory(bb);

            factory.DroneState_MoveRandom.AddTransitions(factory.To_Hunt);
            factory.DroneState_Hunt.AddTransitions(factory.ToEngage, factory.ToMoveRandom);
            factory.DroneState_Engage.AddTransitions(factory.To_Hunt);

            initialState = factory.DroneState_Hunt;
        }
    }
}