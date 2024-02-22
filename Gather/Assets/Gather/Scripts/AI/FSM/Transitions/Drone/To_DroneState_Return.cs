using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{

    public class To_DroneState_Return : FSM_Transition
    {
        private readonly FarmerDrone drone;

        public To_DroneState_Return(Blackboard context, FSM_State next): base(context, next)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
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
