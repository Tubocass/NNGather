using Gather;

namespace Gather.AI.FSM.States
{
    public class FarmerState_Search : FSM_State
    {
        FarmerDrone drone;
        FoodBerry target;
        FoodDetector foodDetector;

        public FarmerState_Search(Blackboard context) : base(context)
        {
            this.drone = context.GetValue<FarmerDrone>(Keys.Unit);
            foodDetector = context.GetValue<FoodDetector>(Keys.FoodDetector);
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
