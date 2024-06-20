using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class SarlacTransitionTo_Awake : FSM_Transition
    {
        public SarlacTransitionTo_Awake(Blackboard context, FSM_State nextState) : base(context, nextState)
        {

        }

        public override bool IsValid()
        {
            return context.GetValue<bool>(Keys.IsNight);
        }
    }
}
