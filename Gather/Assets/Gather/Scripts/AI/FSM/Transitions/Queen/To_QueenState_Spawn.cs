using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_QueenState_Spawn : FSM_Transition
    {
        private readonly Queen queen;
        private readonly QueenFoodManager foodCounter;

        public To_QueenState_Spawn(Queen queen, FSM_State next): base(queen, next)
        {
            this.queen = queen;
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