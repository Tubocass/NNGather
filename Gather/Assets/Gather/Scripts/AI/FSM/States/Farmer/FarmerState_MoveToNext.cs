using gather;
using UnityEngine;


namespace Gather.AI.FSM.States
{
    public class FarmerState_MoveToNext : FSM_State
    {
        FarmerDrone drone;
        Vector2 nextSource;
        bool arrivedAtSource;

        public FarmerState_MoveToNext(Blackboard context) : base(context)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
        }

        public override void EnterState()
        {
            if (nextSource == Vector2.zero || (drone.IsVisitingKnownSources && arrivedAtSource))
            {
                context.SetValue(Configs.ArrivedAtSource, false);
                nextSource = drone.waypoints.Dequeue();
            }

            drone.SetDestination(nextSource);
        }

        public override void Update()
        {
            arrivedAtSource = Vector2.Distance(drone.GetLocation(), nextSource) <= 20;
            context.SetValue(Configs.ArrivedAtSource, arrivedAtSource);

            if (arrivedAtSource && !drone.IsVisitingKnownSources)
            {
                drone.isExploring = true;
            }
        }

    }
}