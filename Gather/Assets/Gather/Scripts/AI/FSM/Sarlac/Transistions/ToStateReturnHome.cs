using gather;
using System.Collections;
using UnityEngine;

namespace Gather.AI
{
    public class ToStateReturnHome : FSM_Transistion
    {
        Sarlac sarlac;
        public ToStateReturnHome(Unit unit, FSM_State nextState) : base(unit, nextState)
        {
            sarlac = unit.GetComponent<Sarlac>();
        }

        public override bool IsValid()
        {
            return !sarlac.isNight && !sarlac.IsAtHome();
        }
    }
}