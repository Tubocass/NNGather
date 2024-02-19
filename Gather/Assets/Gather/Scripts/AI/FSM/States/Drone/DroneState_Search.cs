using gather;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class DroneState_Search : FSM_State
    {
        FarmerDrone drone;
        FoodPellet target;
        FoodDetector foodDetector;
        FoodManager foodManager;
        bool changePath;
        
        public DroneState_Search(FarmerDrone drone)
        {
            this.drone = drone;
            foodDetector = drone.Blackboard.GetValue<FoodDetector>(Configs.FoodDetector);
        }

        public override void EnterState()
        {
            changePath = true;
            foodManager = drone.TeamConfig.FoodManager;
        }

        public override void Update()
        {
            Search();
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
            else
            {
                Vector2 nearestFoodSource = drone.TeamConfig.FoodManager
                    .NearsestFoodSourceLocation(drone.GetLocation());

                if (nearestFoodSource != Vector2.zero 
                    && Vector2.Distance(nearestFoodSource, drone.GetLocation()) >= foodDetector.config.searchDist)
                {
                    drone.SetDestination(nearestFoodSource);
                } 
                else if (!drone.IsMoving || changePath)
                {
                    changePath = false;
                    drone.MoveRandomly(drone.AnchorPoint());
                }
            }
            
        }
    }
}
