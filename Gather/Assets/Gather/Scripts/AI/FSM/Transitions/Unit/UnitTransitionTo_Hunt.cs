using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class UnitTransitionTo_Hunt : FSM_Transition
    {

        public UnitTransitionTo_Hunt(Blackboard context, FSM_State next) : base(context, next)
        {
        }

        public override bool IsValid()
        {
            return !unit.IsMoving && !context.GetValue<bool>(Configs.HasTarget); ;
        }
    }
}