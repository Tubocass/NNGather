using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_Hunt : FSM_Transition
    {
        private readonly FighterDrone drone;

        public To_DroneState_Hunt(FighterDrone drone, FSM_State next): base(drone, next)
        {
            this.drone = drone;
        }

        public override bool IsValid()
        {
            return !drone.HasTarget;
        }

        public override void OnTransition()
        {
        }
    }
}