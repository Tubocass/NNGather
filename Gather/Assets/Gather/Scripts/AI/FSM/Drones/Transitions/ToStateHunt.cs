using gather;

namespace Gather.AI
{
    public class ToStateHunt : FSM_Transistion
    {
        FighterDrone drone;
        FSM_State nextState;
        public ToStateHunt(FighterDrone drone, FSM_State next)
        {
            this.drone = drone;
            this.nextState = next;
        }

        public bool isValid()
        {
            return false;
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