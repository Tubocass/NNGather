using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Controllers
{
    public class FighterFSM_Controller : FSM_Controller
    {
        UnitStateFactory factory;

        protected override void Init()
        {
            FighterDrone drone = GetComponent<FighterDrone>();
            Blackboard bb = drone.Blackboard;
            factory = new UnitStateFactory(bb);

            factory.UnitState_MoveRandom.AddTransitions(factory.To_Hunt);
            factory.UnitState_Hunt.AddTransitions(factory.ToEngage, factory.ToMoveRandom);
            factory.UnitState_Engage.AddTransitions(factory.To_Hunt);

            initialState = factory.UnitState_Hunt;
        }
    }
}