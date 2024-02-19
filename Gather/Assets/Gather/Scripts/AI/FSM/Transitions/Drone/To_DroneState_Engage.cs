using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_Engage : FSM_Transition
    {
        protected Unit drone;

        public To_DroneState_Engage(Unit unit, FSM_State next): base(unit, next)
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