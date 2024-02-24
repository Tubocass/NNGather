using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Controllers
{
    public class SarlacFSM_Controller : FSM_Controller
    {
        Sarlac sarlac;

        protected override void Init()
        {
            sarlac = GetComponent<Sarlac>();
            Blackboard bb = sarlac.Blackboard;
            SarlacStateFactory factory = new SarlacStateFactory(bb);

            factory.SarlacState_Sleep.AddTransitions(factory.To_Hunt);
            factory.UnitState_Hunt.AddTransitions(
                factory.To_Engage, factory.To_Return, factory.ToMoveRandom, factory.To_Sleep
                );
            factory.UnitState_MoveRandom.AddTransitions(factory.To_Hunt, factory.To_Return);
            factory.UnitState_Engage.AddTransitions(factory.To_Hunt, factory.To_Hunt, factory.To_Return);
            factory.UnitState_Return.AddTransitions(factory.To_Sleep, factory.To_Hunt);

            initialState = factory.SarlacState_Sleep;
        }
    }
}