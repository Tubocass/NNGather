using Gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class FarmerTransitionTo_LookForFood : FSM_Transition
    {
        FarmerDrone drone;
        public FarmerTransitionTo_LookForFood(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
            drone = context.GetValue<FarmerDrone>(Keys.Unit);
        }

        public override bool IsValid()
        {
            return drone.IsSearchingForFood;
        }
    }
}