using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class SarlacTransitionTo_Engage : UnitTransitionTo_Engage
    {
        public SarlacTransitionTo_Engage(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
        }

        public override bool IsValid()
        {
            return context.GetValue<bool>(Configs.HasTarget) && context.GetValue<bool>(Configs.IsNight);
        }
    }
}