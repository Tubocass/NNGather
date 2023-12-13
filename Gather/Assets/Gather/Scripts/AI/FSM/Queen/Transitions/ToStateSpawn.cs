using gather;

namespace Gather.AI
{
    public class ToStateSpawn : FSM_Transistion
    {
        Queen queen;
        FSM_State nextState;
        public ToStateSpawn(Queen queen, FSM_State next)
        {
            this.queen = queen;
            this.nextState = next;
        }

        public bool isValid()
        {
            return queen.IsFoodFull() && !queen.IsMoving;
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