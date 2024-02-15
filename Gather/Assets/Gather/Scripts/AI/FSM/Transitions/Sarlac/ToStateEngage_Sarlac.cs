using gather;
using Gather.AI.FSM.States;
using Gather.AI.FSM.Transitions;
using UnityEngine;

namespace Assets.Gather.Scripts.AI.FSM.Transitions
{
    public class ToStateEngage_Sarlac : ToStateEngage
    {
        Sarlac sarlac;
        public ToStateEngage_Sarlac(Unit unit, FSM_State next) : base(unit, next)
        {
            sarlac = unit.GetComponent<Sarlac>();
        }

        public override bool IsValid()
        {
            return sarlac.HasTarget && sarlac.isNight;
        }
    }
}