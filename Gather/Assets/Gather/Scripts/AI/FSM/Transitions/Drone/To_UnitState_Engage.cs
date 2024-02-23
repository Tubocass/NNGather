using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_UnitState_Engage : FSM_Transition
    {

        public To_UnitState_Engage(Blackboard context, FSM_State next): base(context, next)
        {
        }

        public override bool IsValid()
        {
            return context.GetValue<bool>(Configs.HasTarget);
        }
    }
}