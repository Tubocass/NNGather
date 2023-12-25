using gather;

namespace Gather.AI
{
    public class State_Feed : FSM_State
    {
        int targetFoodCount = 10;
        FoodCounter foodCounter;

        public State_Feed(Queen queen, Blackboard context)
        {
       
            foodCounter = context.GetValue<FoodCounter>(Configs.FoodCounter);
        }

        public override void EnterState()
        {
            //targetFoodCount = context.GetValue<int>(Configs.TargetFoodCount);
        }

        public override void Update()
        {

        }

        public override void ExitState()
        {
        }
    }
}