using gather;

namespace Gather.AI
{
    public class ToStateEngage : FSM_Transistion
    {
        Drone drone;
        FSM_State nextState;
        public ToStateEngage(Drone drone, FSM_State next)
        {
            this.drone = drone;
            this.nextState = next;
        }

        public bool isValid()
        {
            return drone.hasTarget;
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