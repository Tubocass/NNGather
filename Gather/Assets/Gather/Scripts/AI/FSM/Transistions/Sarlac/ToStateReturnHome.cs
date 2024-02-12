using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
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