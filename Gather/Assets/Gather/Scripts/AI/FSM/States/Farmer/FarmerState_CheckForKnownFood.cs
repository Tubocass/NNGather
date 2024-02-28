using Gather;
using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class FarmerState_CheckForKnownFood : FSM_State
    {
        FarmerDrone drone;

        public FarmerState_CheckForKnownFood(Blackboard context):base(context)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
        }

        public override void EnterState()
        {
            List<Vector2> sources = new List<Vector2>(drone.TeamConfig.FoodManager.GetFoodSources());

            
            if (sources.Count > 0) 
            {
                drone.isExploring = false;
                //sources.Sort(CompareDistanceToDrone);
                //sources.ForEach(source => drone.waypoints.Enqueue(source));
                
                //  Daisy Chain
                Vector2 next = drone.GetLocation();

                for (int i = 0; i < sources.Count; ++i)
                {
                    next = TargetSystem.TargetNearest(next, sources);
                    sources.Remove(next);
                    drone.waypoints.Enqueue(next);
                }
            }
        }

        int CompareDistanceToDrone(Vector2 a,  Vector2 b)
        {
            Vector2 droneLocation = drone.GetLocation();
            if (Vector2.Distance(a, droneLocation) < (Vector2.Distance(b, droneLocation)))
            {
                return -1;
            } else if (Vector2.Distance(a, droneLocation) > (Vector2.Distance(b, droneLocation)))
            {
                return 1;
            } else return 0;
        }
    }
}