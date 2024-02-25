using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class SarlacTransitionTo_Hunt : FSM_Transition
    {

        public SarlacTransitionTo_Hunt(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
        }

        public override bool IsValid()
        {
            return !context.GetValue<bool>(Configs.HasTarget);
        }
    }
}
