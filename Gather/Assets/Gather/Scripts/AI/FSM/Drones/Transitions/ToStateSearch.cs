using gather;
using UnityEngine;

namespace Gather.AI
{
    public class ToStateSearch : FSM_Transistion
    {
        FarmerDrone drone;
        FSM_State nextState;
        public ToStateSearch(FarmerDrone drone, FSM_State next)
        {
            this.drone = drone;
            this.nextState = next;
        }

        public bool isValid()
        {
            return !drone.GetEnemyDetected() && !drone.IsCarryingFood();
        }

        public FSM_State GetNextState()
        {
            OnTransition();
            return nextState;
        }

        public void OnTransition()
        {
            //Debug.Log("Start search");
        }
    }
}