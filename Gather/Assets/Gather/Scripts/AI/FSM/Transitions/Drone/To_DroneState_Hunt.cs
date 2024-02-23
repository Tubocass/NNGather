using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_Hunt : FSM_Transition
    {

        public To_DroneState_Hunt(Blackboard context, FSM_State next) : base(context, next)
        {
        }

        public override bool IsValid()
        {
            return !unit.IsMoving && !context.GetValue<bool>(Configs.HasTarget); ;
        }
    }
}