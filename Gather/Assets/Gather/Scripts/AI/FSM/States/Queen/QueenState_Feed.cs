using Gather;

namespace Gather.AI.FSM.States
{
    public class QueenState_Feed : FSM_State
    {
        //int targetFoodCount = 10;
        QueenFoodManager foodCounter;

        public QueenState_Feed(Blackboard context) : base(context)
        {
            Queen queen = context.GetValue<Queen>(Configs.Unit);
            foodCounter = queen.GetComponent<QueenFoodManager>();
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