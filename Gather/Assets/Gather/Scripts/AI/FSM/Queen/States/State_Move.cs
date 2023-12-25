using System.Collections.Generic;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Move : FSM_State
    {
        Queen queen;
        Blackboard context;
        Queue<Vector2> foods;
        Vector2 targetDestination = Vector2.zero;
        FoodCounter foodCounter;

        public State_Move(Queen queen, Blackboard context)
        {
            this.queen = queen;
            this.context = context;
            foodCounter = context.GetValue<FoodCounter>(Configs.FoodCounter);
            //foods = context.GetValue<Queue<Vector2>>(Configs.FoodLocations);
        }

        public override void EnterState()
        {
            if(targetDestination != Vector2.zero)
            {
                queen.SetDestination(targetDestination);
            }else
            {
                MoveToFoodCenter();
            }
        }

        public void SetTargetDestination(Vector2 target)
        {
            targetDestination = target;
        }

        void MoveToFoodCenter()
        {
            targetDestination = foodCounter.FoodCenter();
            
            queen.SetDestination(targetDestination);
        }

        public override void Update()
        {
         
        }

        public override void ExitState()
        {
            targetDestination = Vector2.zero;
        }
    }
}