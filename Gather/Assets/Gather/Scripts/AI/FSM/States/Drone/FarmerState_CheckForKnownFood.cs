﻿using gather;
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
    }
}