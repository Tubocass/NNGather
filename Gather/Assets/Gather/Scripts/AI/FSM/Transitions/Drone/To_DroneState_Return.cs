using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{

    public class To_DroneState_Return : FSM_Transition
    {
        private readonly FarmerDrone drone;

        public To_DroneState_Return(FarmerDrone drone, FSM_State next): base(drone, next)
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
