using gather;

namespace Gather.AI
{
    public class ToStateFeed : FSM_Transistion
    {
        Queen queen;
        FSM_State nextState;
        FoodCounter counter;

        public ToStateFeed(Queen queen, FSM_State next)
        {
            this.queen = queen;
            this.nextState = next;
            counter = queen.GetFoodCounter();
        }

        public bool isValid()
        {
            return !queen.GetEnemyDetected() 
                && counter.IsFoodLow() 
                && !queen.IsMoving;
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