using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_MoveRandom : FSM_Transition
    {
        FarmerDrone drone;

        public To_DroneState_MoveRandom(FarmerDrone drone, FSM_State nextState) : base(drone, nextState)
        {
            this.drone = drone;
        }

        public override bool IsValid()
        {
            return !drone.GetEnemyDetected() && !drone.IsMoving && drone.sourcesToVist.Count == 0 && !drone.HasTarget && !drone.IsCarryingFood();
        }
    }
}