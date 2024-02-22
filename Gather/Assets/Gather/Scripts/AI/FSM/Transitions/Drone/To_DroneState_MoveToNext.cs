using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_MoveToNext : FSM_Transition
    {
        FarmerDrone drone;

        public To_DroneState_MoveToNext(Blackboard context, FSM_State next) : base(context, next)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
        }

        public override bool IsValid()
        {
            return !drone.IsMoving && drone.IsVisitingKnownSources;
        }
    }
}