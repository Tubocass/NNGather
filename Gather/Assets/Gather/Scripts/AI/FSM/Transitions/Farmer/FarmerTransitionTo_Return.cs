using Gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{

    public class FarmerTransitionTo_Return : FSM_Transition
    {
        private readonly FarmerDrone drone;

        public FarmerTransitionTo_Return(Blackboard context, FSM_State next): base(context, next)
        {
            this.drone = context.GetValue<FarmerDrone>(Keys.Unit);
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
