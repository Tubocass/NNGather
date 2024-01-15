using gather;

namespace Gather.AI
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
            return !drone.hasTarget;
        }

        public override void OnTransition()
        {
        }
    }
}