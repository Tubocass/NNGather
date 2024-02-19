using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_Search : FSM_Transition
    {
        private readonly FarmerDrone drone;

        public To_DroneState_Search(FarmerDrone drone, FSM_State nextState): base(drone, nextState)
        {
            this.drone = drone;
        }

        public override bool IsValid()
        {
            return !drone.GetEnemyDetected() && !drone.IsCarryingFood() && !drone.HasTarget;
        }

        public override void OnTransition()
        {
            //Debug.Log("Start search");
        }
    }
}