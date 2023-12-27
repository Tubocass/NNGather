using System.Collections.Generic;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Move : FSM_State
    {
        Queen queen;
        //Blackboard context;
        FoodCounter foodCounter;

        public State_Move(Queen queen, Blackboard context)
        {
            this.queen = queen;
            //this.context = context;
            foodCounter = context.GetValue<FoodCounter>(Configs.FoodCounter);
        }

        public override void EnterState()
        {
            MoveToFoodCenter();
        }

        void MoveToFoodCenter()
        {
            queen.SetDestination(foodCounter.FoodCenter());
        }

        public override void Update()
        {
         
        }

        public override void ExitState()
        {
        }
    }
}