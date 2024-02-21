using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_MoveToNext : FSM_Transition
    {
        FarmerDrone drone;

        public To_DroneState_MoveToNext(FarmerDrone drone, FSM_State nextState) : base(drone, nextState)
        {
            this.drone = drone;
        }

        public override bool IsValid()
        {
            return !drone.IsMoving && drone.IsSearchingForFood && drone.IsVisitingKnownSources;
        }
    }
}