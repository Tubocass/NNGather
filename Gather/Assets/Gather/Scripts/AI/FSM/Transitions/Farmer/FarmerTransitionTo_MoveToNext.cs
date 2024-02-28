using Gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class FarmerTransitionTo_MoveToNext : FSM_Transition
    {
        FarmerDrone drone;

        public FarmerTransitionTo_MoveToNext(Blackboard context, FSM_State next) : base(context, next)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
        }

        public override bool IsValid()
        {
            return !drone.IsMoving && drone.IsSearchingForFood && drone.IsVisitingKnownSources;
        }
    }
}