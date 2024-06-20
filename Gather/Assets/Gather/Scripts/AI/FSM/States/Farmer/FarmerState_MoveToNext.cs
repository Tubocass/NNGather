using Gather;
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
            this.drone = context.GetValue<FarmerDrone>(Keys.Unit);
        }

        public override void EnterState()
        {
            if (nextSource == Vector2.zero || (drone.IsVisitingKnownSources && arrivedAtSource))
            {
                context.SetValue(Keys.ArrivedAtSource, false);
                nextSource = drone.waypoints.Dequeue();
            }

            drone.SetDestination(nextSource);
        }

        public override void Update()
        {
            arrivedAtSource = Vector2.Distance(drone.GetLocation(), nextSource) <= 20;
            context.SetValue(Keys.ArrivedAtSource, arrivedAtSource);

            if (arrivedAtSource && !drone.IsVisitingKnownSources)
            {
                drone.isExploring = true;
            }
        }

    }
}