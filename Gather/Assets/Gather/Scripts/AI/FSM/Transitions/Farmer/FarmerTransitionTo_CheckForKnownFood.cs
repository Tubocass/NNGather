using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class FarmerTransitionTo_CheckForKnownFood : FSM_Transition
    {
        FarmerDrone drone;

        public FarmerTransitionTo_CheckForKnownFood(Blackboard context, FSM_State next) : base(context, next)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
        }

        public override bool IsValid()
        {
            return drone.CanCheckForSources;
        }
    }
}