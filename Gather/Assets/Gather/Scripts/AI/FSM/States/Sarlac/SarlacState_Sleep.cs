
using Gather;

namespace Gather.AI.FSM.States
{

    public class SarlacState_Sleep : FSM_State
    {
        Sarlac sarlac;
        public SarlacState_Sleep(Blackboard context) : base(context)
        {
            sarlac = context.GetValue<Sarlac>(Keys.Unit);
        }

        public override void EnterState()
        {
            sarlac.Sleep();
        }

    }
}
