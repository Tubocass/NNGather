using gather;
using UnityEngine;

namespace Gather.AI {

    public class ToStateReturn : FSM_Transistion
    {
        FarmerDrone drone;
        FSM_State nextState;
        public ToStateReturn(FarmerDrone drone, FSM_State next)
        {
            this.drone = drone;
            this.nextState = next;
        }

        public bool isValid()
        {
            return drone.IsCarryingFood() && !drone.GetEnemyDetected();
        }

        public FSM_State GetNextState()
        {
            OnTransition();
            return nextState;
        }

        public void OnTransition()
        {
        }
    }
}
