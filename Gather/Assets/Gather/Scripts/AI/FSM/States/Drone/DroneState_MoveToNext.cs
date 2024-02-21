using gather;
using UnityEngine;


namespace Gather.AI.FSM.States
{
    public class DroneState_MoveToNext : FSM_State
    {
        FarmerDrone drone;

        public DroneState_MoveToNext (FarmerDrone drone)
        {
            this.drone = drone;
        }

        public override void EnterState()
        {
            Debug.Log("MoveToNext");
            drone.SetDestination(drone.sourcesToVist.Dequeue());
        }

    }
}