using gather;
using Gather.AI.FSM.States;
using Gather.AI.FSM.Transitions;
using UnityEngine;

namespace Assets.Gather.Scripts.AI.FSM.Transitions
{
    public class To_SarlacState_Engage : To_DroneState_Engage
    {
        Sarlac sarlac;
        public To_SarlacState_Engage(Unit unit, FSM_State next) : base(unit, next)
        {
            sarlac = unit.GetComponent<Sarlac>();
        }

        public override bool IsValid()
        {
            return sarlac.HasTarget && sarlac.isNight;
        }
    }
}