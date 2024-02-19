using gather;

namespace Gather.AI.FSM.States
{
    public class DroneState_Search : FSM_State
    {
        FarmerDrone drone;
        FoodPellet target;
        FoodDetector foodDetector;
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
                drone.Blackboard.SetValue<ITargetable>(Configs.Target, target);
                drone.SetHasTarget(true);
            }
            else if (!drone.IsMoving || changePath)
            {
                changePath = false;
                drone.MoveRandomly(drone.AnchorPoint());
            }
        }
    }
}
