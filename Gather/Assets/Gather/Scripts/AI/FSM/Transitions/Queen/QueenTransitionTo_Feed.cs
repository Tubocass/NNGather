using Gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class QueenTransitionTo_Feed : FSM_Transition
    {
        private readonly Queen queen;
        private readonly QueenFoodManager foodCounter;

        public QueenTransitionTo_Feed(Blackboard context, FSM_State next) : base(context, next)
        {
            this.queen = context.GetValue<Queen>(Configs.Unit);
            foodCounter = queen.GetComponent<QueenFoodManager>();
        }

        public override bool IsValid()
        {
            return !queen.GetEnemyDetected() 
                && foodCounter.IsFoodLow() 
                && !queen.IsMoving;
        }

        public override void OnTransition()
        {
        }
    }
}