using gather;

namespace Gather.AI
{
    public class ToStateSearch : FSM_Transistion
    {
        private readonly FarmerDrone drone;

        public ToStateSearch(FarmerDrone drone, FSM_State nextState): base(drone, nextState)
        {
            this.drone = drone;
        }

        public override bool IsValid()
        {
            return !drone.GetEnemyDetected() && !drone.IsCarryingFood() && !drone.hasTarget;
        }

        public override void OnTransition()
        {
            //Debug.Log("Start search");
        }
    }
}