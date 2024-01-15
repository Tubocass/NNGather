using gather;

namespace Gather.AI
{
    public class ToStateFeed : FSM_Transistion
    {
        private readonly Queen queen;
        private readonly FoodCounter counter;

        public ToStateFeed(Queen queen, FSM_State next) : base(queen, next)
        {
            this.queen = queen;
            counter = queen.GetFoodCounter();
        }

        public override bool IsValid()
        {
            return !queen.GetEnemyDetected() 
                && counter.IsFoodLow() 
                && !queen.IsMoving;
        }

        public override void OnTransition()
        {
        }
    }
}