using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class ToStateAwake : FSM_Transition
    {
        private readonly Sarlac sarlac;
        public ToStateAwake(Sarlac sarlac, FSM_State nextState) : base(sarlac, nextState)
        {
            this.sarlac = sarlac;
        }

        public override bool IsValid()
        {
            return sarlac.isNight && !sarlac.HasTarget;
        }

        public override void OnTransition()
        {
        }
    }
}