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
            counter = queen.GetBlackboard().GetValue<FoodCounter>(Configs.FoodCounter);
        }

        public bool isValid()
        {
            return counter.AverageDistanceFromFood(queen.Location()) > 30;
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