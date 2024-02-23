using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_FarmerState_MoveRandom : FSM_Transition
    {
        FarmerDrone drone;

        public To_FarmerState_MoveRandom(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
        }

        public override bool IsValid()
        {
            return !drone.IsMoving && drone.IsSearchingForFood && !drone.IsVisitingKnownSources && !drone.CanCheckForSources;
        }
    }
}