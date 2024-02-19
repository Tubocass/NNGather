using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_SarlacState_Return : FSM_Transition
    {
        Sarlac sarlac;
        public To_SarlacState_Return(Unit unit, FSM_State nextState) : base(unit, nextState)
        {
            sarlac = unit.GetComponent<Sarlac>();
        }

        public override bool IsValid()
        {
            return !sarlac.isNight && !sarlac.IsAtHome();
        }
    }
}