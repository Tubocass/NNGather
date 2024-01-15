﻿using gather;

namespace Gather.AI
{
    public class ToStateFlee : FSM_Transistion
    {
        public ToStateFlee(Unit unit, FSM_State next): base(unit, next)
        {
            this.unit = unit;
        }

        public override bool IsValid()
        {
            return unit.GetEnemyDetected();
        }

        public override void OnTransition()
        {
        }
    }
}