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
            //Debug.Log("Food left to eat " + (targetFoodCount - foodCounter.amount));
            if(foodCounter.amount >= targetFoodCount)
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