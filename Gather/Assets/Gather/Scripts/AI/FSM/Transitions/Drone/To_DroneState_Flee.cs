using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_Flee : FSM_Transition
    {
        public To_DroneState_Flee(Unit unit, FSM_State next): base(unit, next)
        {
            this.unit = unit;
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