using gather;

namespace Gather.AI {

    public class ToStateReturn : FSM_Transistion
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
