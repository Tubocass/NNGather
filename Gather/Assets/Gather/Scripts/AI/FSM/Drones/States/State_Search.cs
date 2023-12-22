using System.Collections.Generic;
using gather;

namespace Gather.AI
{
    public class State_Search : FSM_State
    {
        List<FoodPellet> foodPellets = new List<FoodPellet>();
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
            foodPellets.Clear();
        }

        public override string GetStateName()
        {
            return States.search;
        }

        void Search()
        {
            foodDetector.Detect();
            foodPellets = foodDetector.GetFoodList();

            if (foodPellets.Count > 0)
            {
                target = TargetSystem.TargetNearest(drone.Location(), foodPellets);
                context.SetValue<ITarget>(Configs.Target, target);
                drone.hasTarget = true;
            }
            else if (!drone.IsMoving || changePath)
            {
                changePath = false;
                drone.MoveRandomly();
            }
        }
    }
}
