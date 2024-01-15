using gather;

namespace Gather.AI
{
    public class ToStateEngage : FSM_Transistion
    {
        Drone drone;

        public ToStateEngage(Drone drone, FSM_State next): base(drone, next)
        {
            this.drone = drone;
        }

        public override bool IsValid()
        {
            return drone.hasTarget;
        }

        public override void OnTransition()
        {
        }
    }
}