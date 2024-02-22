using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_Flee : FSM_Transition
    {
        public To_DroneState_Flee(Blackboard context, FSM_State next) : base(context, next)
        {
        }

        public override bool IsValid()
        {
            return unit.GetEnemyDetected();
        }

        public override void OnTransition()
        {
        }
    }
}