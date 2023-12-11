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
        int targetFoodCount = 10;
        Counter foodCounter;
        int ticks = 0;

        public State_Feed(Queen queen, Blackboard context)
        {
            this.queen = queen;
            this.context = context;
            foodCounter = context.GetValue<Counter>(Configs.FoodCounter);
        }

        public void EnterState()
        {
            //targetFoodCount = context.GetValue<int>(Configs.TargetFoodCount);
        }

        public void AssesSituation()
        {
            ticks++;
            if (foodCounter.amount >= targetFoodCount)
            {
                Finished?.Invoke();
            }else if(ticks >= 64)
            {
                Finished?.Invoke();
            }
        }

        public void ExitState()
        {
        }

        string IBehaviorState.ToString()
        {
            return States.feed;
        }
    }
}