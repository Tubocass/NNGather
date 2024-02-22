using gather;
using UnityEngine;


namespace Gather.AI.FSM.States
{
    public class DroneState_MoveToNext : FSM_State
    {
        FarmerDrone drone;
        Vector2 nextSource;
        bool arrivedAtSource;

        public DroneState_MoveToNext(Blackboard context) : base(context)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
        }

        public override void EnterState()
        {
            Debug.Log("MoveToNext");
            if (nextSource == Vector2.zero || arrivedAtSource)
            {
                context.SetValue("arrivedAtSource", false);
                nextSource = drone.sourcesToVist.Dequeue();
            }

            drone.SetDestination(nextSource);
        }

        public override void Update()
        {
            arrivedAtSource = Vector2.Distance(drone.GetLocation(), nextSource) <= 1;
            context.SetValue("arrivedAtSource", arrivedAtSource);

            if (arrivedAtSource && !drone.IsVisitingKnownSources)
            {
                drone.isExploring = true;
            }
        }

    }
}