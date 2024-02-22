﻿using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_QueenState_Feed : FSM_Transition
    {
        private readonly Queen queen;
        private readonly QueenFoodManager foodCounter;

        public To_QueenState_Feed(Blackboard context, FSM_State next) : base(context, next)
        {
            this.queen = context.GetValue<Queen>(Configs.Unit);
            foodCounter = queen.GetComponent<QueenFoodManager>();
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