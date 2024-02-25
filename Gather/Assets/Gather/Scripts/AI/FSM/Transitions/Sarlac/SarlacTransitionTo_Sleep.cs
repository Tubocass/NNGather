using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class SarlacTransitionTo_Sleep : FSM_Transition
    {
        private readonly Sarlac sarlac;

        public SarlacTransitionTo_Sleep(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
            sarlac = unit.GetComponent<Sarlac>();
        }

        public override bool IsValid()
        {
            return !context.GetValue<bool>(Configs.IsNight) && sarlac.IsAtHome();
        }

        public override void OnTransition()
        {
        }
    }
}
