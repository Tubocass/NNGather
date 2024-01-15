using gather;

namespace Gather.AI
{
    public class State_Search : FSM_State
    {
        Drone drone;
        FoodPellet target;
        Blackboard context;
        FoodDetector foodDetector;
        bool changePath;
        
        public State_Search(Drone drone, Blackboard context)
        {
            this.drone = drone;
            this.context = context;
            foodDetector = context.GetValue<FoodDetector>(Configs.FoodDetector);
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
            if (foodDetector.Detect())
            {
                target = TargetSystem.TargetNearest(drone.Location(), foodDetector.GetFoodList());
                context.SetValue<ITarget>(Configs.Target, target);
                drone.hasTarget = true;
            }
            else if (!drone.IsMoving || changePath)
            {
                changePath = false;
                drone.MoveRandomly(drone.AnchorPoint());
            }
        }
    }
}
