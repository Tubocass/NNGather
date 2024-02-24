using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class UnitTransitionTo_Engage : FSM_Transition
    {

        public UnitTransitionTo_Engage(Blackboard context, FSM_State next): base(context, next)
        {
        }

        public override bool IsValid()
        {
            return context.GetValue<bool>(Configs.HasTarget);
        }
    }
}