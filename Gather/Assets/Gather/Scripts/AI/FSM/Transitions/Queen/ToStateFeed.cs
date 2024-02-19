﻿using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class ToStateFeed : FSM_Transition
    {
        private readonly Queen queen;
        private readonly FoodManager foodCounter;

        public ToStateFeed(Queen queen, FSM_State next) : base(queen, next)
        {
            this.queen = queen;
            foodCounter = queen.GetComponent<FoodManager>();
        }

        public override bool IsValid()
        {
            return !queen.GetEnemyDetected() 
                && foodCounter.IsFoodLow() 
                && !queen.IsMoving;
        }

        public override void OnTransition()
        {
        }
    }
}