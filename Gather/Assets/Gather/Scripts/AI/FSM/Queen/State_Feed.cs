using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Feed : IBehaviorState
    {
        public GameEvent Finished;
        Queen queen;
        Blackboard context;
        int targetFoodCount;
        Counter foodCount;

        public State_Feed(Queen queen, Blackboard context)
        {
            this.queen = queen;
            this.context = context;
        }

        public void EnterState()
        {
            targetFoodCount = context.GetValue<int>(Configs.TargetFoodCount);
            foodCount = context.GetValue<Counter>(Configs.FoodCounter);
        }

        public void AssesSituation()
        {
            if(foodCount.amount >= targetFoodCount)
            {
                Finished?.Invoke();
            }
        }

        public void ExitState()
        {
        }
    }
}