using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{

    public class ToStateReturn : FSM_Transition
    {
        private readonly FarmerDrone drone;

        public ToStateReturn(FarmerDrone drone, FSM_State next): base(drone, next)
        {
            this.drone = drone;
        }

        public override bool IsValid()
        {
            return drone.IsCarryingFood() && !drone.GetEnemyDetected();
        }

        public override void OnTransition()
        {
        }
    }
}
