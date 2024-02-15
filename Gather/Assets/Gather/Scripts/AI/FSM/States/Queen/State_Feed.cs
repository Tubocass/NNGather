using gather;

namespace Gather.AI.FSM.States
{
    public class State_Feed : FSM_State
    {
        //int targetFoodCount = 10;
        FoodManager foodCounter;

        public State_Feed(Queen queen)
        {

            foodCounter = queen.GetComponent<FoodManager>();
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