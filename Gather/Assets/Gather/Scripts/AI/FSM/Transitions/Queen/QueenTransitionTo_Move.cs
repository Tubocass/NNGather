using Gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class QueenTransitionTo_Move : FSM_Transition
    {
        private readonly Queen queen;
        private readonly QueenFoodManager foodCounter;

        public QueenTransitionTo_Move(Blackboard context, FSM_State next) : base(context, next)
        {
            this.queen = context.GetValue<Queen>(Keys.Unit);
            foodCounter = queen.GetComponent<QueenFoodManager>();
        }

        public override bool IsValid()
        {
            return foodCounter.AverageDistanceFromFood(queen.GetLocation()) > 20;
        }

        public override void OnTransition()
        {
        }
    }
}