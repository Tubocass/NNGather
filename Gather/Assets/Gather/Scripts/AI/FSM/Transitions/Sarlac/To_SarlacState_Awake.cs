using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_SarlacState_Awake : FSM_Transition
    {
        private readonly Sarlac sarlac;

        public To_SarlacState_Awake(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
            sarlac = unit.GetComponent<Sarlac>();
        }

        public override bool IsValid()
        {
            return sarlac.isNight && !context.GetValue<bool>(Configs.HasTarget);
        }

        public override void OnTransition()
        {
        }
    }
}
