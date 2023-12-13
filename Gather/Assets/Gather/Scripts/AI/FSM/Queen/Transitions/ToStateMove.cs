using gather;

namespace Gather.AI
{
    public class ToStateMove : FSM_Transistion
    {
        Queen queen;
        FSM_State nextState;
        public ToStateMove(Queen queen, FSM_State next)
        {
            this.queen = queen;
            this.nextState = next;
        }

        public bool isValid()
        {
            return queen.AverageDistanceFromFood() > 20;
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