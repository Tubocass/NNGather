using gather;
using UnityEngine;


namespace Gather.AI.FSM.States
{
    public class DroneState_MoveToNext : FSM_State
    {
        FarmerDrone drone;
        Vector2 nextSource;


        public DroneState_MoveToNext (FarmerDrone drone)
        {
            this.drone = drone;
        }

        public override void EnterState()
        {
            drone.arrivedAtSource = false;
            Debug.Log("MoveToNext");
            nextSource = drone.sourcesToVist.Dequeue();

            drone.SetDestination(nextSource);
        }

        public override void Update()
        {
            drone.arrivedAtSource = Vector2.Distance(drone.GetLocation(), nextSource) <= 1;
            if (drone.arrivedAtSource && !drone.IsVisitingKnownSources)
            {
                drone.isExploring = true;
            }
        }

    }
}