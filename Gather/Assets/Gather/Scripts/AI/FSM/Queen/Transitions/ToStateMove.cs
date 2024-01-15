using gather;

namespace Gather.AI
{
    public class ToStateMove : FSM_Transistion
    {
        private readonly Queen queen;
        private readonly FoodCounter counter;

        public ToStateMove(Queen queen, FSM_State next) : base(queen, next)
        {
            this.queen = queen;
            counter = queen.GetFoodCounter();
        }

        public override bool IsValid()
        {
            return counter.AverageDistanceFromFood(queen.Location()) > 20;
        }

        public override void OnTransition()
        {
        }
    }
}