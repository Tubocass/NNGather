using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class ToStateHunt : FSM_Transistion
    {
        private readonly FighterDrone drone;

        public ToStateHunt(FighterDrone drone, FSM_State next): base(drone, next)
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