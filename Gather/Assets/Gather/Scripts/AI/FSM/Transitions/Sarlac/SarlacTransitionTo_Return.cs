using Gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class SarlacTransitionTo_Return : FSM_Transition
    {
        Sarlac sarlac;
        public SarlacTransitionTo_Return(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
            sarlac = unit.GetComponent<Sarlac>();
        }

        public override bool IsValid()
        {
            return !context.GetValue<bool>(Keys.IsNight);
        }
    }
}