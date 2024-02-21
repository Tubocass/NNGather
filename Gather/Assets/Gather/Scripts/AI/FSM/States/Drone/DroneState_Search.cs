using gather;
using System.Collections.Generic;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class DroneState_Search : FSM_State
    {
        FarmerDrone drone;
        FoodBerry target;
        FoodDetector foodDetector;
        Queue<Vector2> recentlyVisitedSpots = new Queue<Vector2>();
        bool changePath;
        
        public DroneState_Search(FarmerDrone drone)
        {
            this.drone = drone;
            foodDetector = drone.Blackboard.GetValue<FoodDetector>(Configs.FoodDetector);
        }

        public override void EnterState()
        {
            Debug.Log("Search");
            changePath = true;
            Search();

        }

        public override void Update()
        {
        }

        public override void ExitState()
        {
            target = null;
        }

        void Search()
        {
            foodDetector.Detect();

            if (foodDetector.DetectedSomething)
            {
                target = TargetSystem.TargetNearest(drone.GetLocation(), foodDetector.GetFoodList());
                drone.TargetFood(target);
            } 
            //else if (!drone.IsMoving || changePath)
            //{
            //    changePath = false;
            //    drone.MoveRandomly(drone.AnchorPoint());
            //}
        }
    }
}
