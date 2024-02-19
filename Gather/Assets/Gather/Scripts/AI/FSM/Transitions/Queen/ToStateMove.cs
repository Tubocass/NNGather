﻿using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class ToStateMove : FSM_Transition
    {
        private readonly Queen queen;
        private readonly FoodManager foodCounter;

        public ToStateMove(Queen queen, FSM_State next) : base(queen, next)
        {
            this.queen = queen;
            foodCounter = queen.GetComponent<FoodManager>();
        }

        public override bool IsValid()
        {
            return foodCounter.AverageDistanceFromFood(queen.GetLocation()) > 20;
        }

        public override void OnTransition()
        {
        }
    }
}