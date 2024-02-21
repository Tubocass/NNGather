using gather;
using System.Collections.Generic;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class DroneState_CheckForKnownFood : FSM_State
    {
        FarmerDrone drone;

        public DroneState_CheckForKnownFood(FarmerDrone drone) 
        {
            this.drone = drone;
        }

        public override void EnterState()
        {
            Debug.Log("CheckForKnownFood");
            List<Vector2> sources = drone.TeamConfig.FoodManager.GetFoodSources();
            if (sources.Count > 0)
            {
                sources.Sort(CompareDistanceToDrone);
                sources.ForEach(source => drone.sourcesToVist.Enqueue(source));
                drone.isExploring = false;
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

        public override void ExitState()
        {
            Debug.Log("source count :" + drone.sourcesToVist.Count);
        }

    }
}