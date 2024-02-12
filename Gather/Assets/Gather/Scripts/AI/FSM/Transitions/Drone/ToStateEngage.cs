using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class ToStateEngage : FSM_Transition
    {
        Unit drone;

        public ToStateEngage(Unit unit, FSM_State next): base(unit, next)
        {
            this.drone = unit;
        }

        public override bool IsValid()
        {
            return drone.HasTarget;
        }

        public override void OnTransition()
        {
        }
    }
}