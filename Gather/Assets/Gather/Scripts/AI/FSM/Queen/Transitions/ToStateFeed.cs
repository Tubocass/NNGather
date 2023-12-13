using gather;

namespace Gather.AI
{
    public class ToStateFeed : FSM_Transistion
    {
        Queen queen;
        FSM_State nextState;
        public ToStateFeed(Queen queen, FSM_State next)
        {
            this.queen = queen;
            this.nextState = next;
        }

        public bool isValid()
        {
            return queen.IsFoodLow();
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