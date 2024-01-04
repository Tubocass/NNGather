using gather;

namespace Gather.AI
{
    public class ToStateMove : FSM_Transistion
    {
        Queen queen;
        FSM_State nextState;
        FoodCounter counter;

        public ToStateMove(Queen queen, FSM_State next)
        {
            this.queen = queen;
            this.nextState = next;
            counter = queen.GetFoodCounter();
        }

        public bool isValid()
        {
            return counter.AverageDistanceFromFood(queen.Location()) > 20;
        }

        public FSM_State GetNextState()
        {
            return nextState;
        }

        public void OnTransition()
        {
        }
    }
}