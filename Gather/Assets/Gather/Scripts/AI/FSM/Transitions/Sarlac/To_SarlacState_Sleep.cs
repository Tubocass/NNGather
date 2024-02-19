using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{

    public class To_SarlacState_Sleep : FSM_Transition
    {
        private readonly Sarlac sarlac;
        public To_SarlacState_Sleep(Sarlac sarlac, FSM_State nextState) : base(sarlac, nextState)
        {
            this.sarlac = sarlac;
        }

        public override bool IsValid()
        {
            return !sarlac.isNight && sarlac.IsAtHome();
        }

        public override void OnTransition()
        {
        }
    }
}
