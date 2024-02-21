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
        float recentTimer = 30f;
        bool changePath;
        
        public DroneState_Search(FarmerDrone drone)
        {
            this.drone = drone;
            foodDetector = drone.Blackboard.GetValue<FoodDetector>(Configs.FoodDetector);
        }

        public override void EnterState()
        {
            changePath = true;
        }

        public override void Update()
        {
            recentTimer -= Time.deltaTime;
            if (recentTimer <= 0f) 
            {
                recentTimer = 30f;
                recentlyVisitedSpots.Clear();
            }
         
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
            else if (!drone.IsMoving || changePath)
            {
                changePath = false;
                drone.MoveRandomly(drone.AnchorPoint());
            }
            //else
            //{
            //    List<Vector2> sources = drone.TeamConfig.FoodManager.GetFoodSources();
            //    sources.ForEach(s =>
            //    {
            //        if (recentlyVisitedSpots.Contains(s))
            //        {
            //            sources.Remove(s);
            //        }
            //    });

            //    Vector2 nearestFoodSource = TargetSystem.TargetNearest(drone.GetLocation(), sources);

            //    if (nearestFoodSource != Vector2.zero 
            //        && Vector2.Distance(nearestFoodSource, drone.GetLocation()) >= foodDetector.config.searchDist)
            //    {
            //        drone.SetDestination(nearestFoodSource);
            //    } 

            //}

        }
    }
}
