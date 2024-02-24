using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class UnitTransitionTo_Flee : FSM_Transition
    {
        public UnitTransitionTo_Flee(Blackboard context, FSM_State next) : base(context, next)
        {
        }

        public override bool IsValid()
        {
            return unit.GetEnemyDetected();
        }

        public override void OnTransition()
        {
        }
    }
}