using gather;

namespace Gather.AI.FSM.States
{
    public class DroneState_Search : FSM_State
    {
        FarmerDrone drone;
        FoodBerry target;
        FoodDetector foodDetector;

        public DroneState_Search(Blackboard context) : base(context)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
            foodDetector = context.GetValue<FoodDetector>(Configs.FoodDetector);
        }

        public override void EnterState()
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
        }
    }
}
