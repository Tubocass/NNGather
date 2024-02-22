using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{

    public class To_SarlacState_Sleep : FSM_Transition
    {
        private readonly Sarlac sarlac;

        public To_SarlacState_Sleep(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
            sarlac = unit.GetComponent<Sarlac>();
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
