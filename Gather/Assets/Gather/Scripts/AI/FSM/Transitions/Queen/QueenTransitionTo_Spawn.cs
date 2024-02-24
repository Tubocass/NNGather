using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class QueenTransitionTo_Spawn : FSM_Transition
    {
        private readonly Queen queen;
        private readonly QueenFoodManager foodCounter;

        public QueenTransitionTo_Spawn(Blackboard context, FSM_State next): base(context, next)
        {
            this.queen = context.GetValue<Queen>(Configs.Unit);
            foodCounter = queen.GetComponent<QueenFoodManager>();
        }

        public override bool IsValid()
        {
            return !queen.GetEnemyDetected() 
                && !foodCounter.IsFoodLow()
                && !queen.IsMoving;
        }

        public override void OnTransition()
        {
        }
    }
}