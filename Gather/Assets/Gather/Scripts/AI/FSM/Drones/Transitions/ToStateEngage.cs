using gather;

namespace Gather.AI
{
    public class ToStateEngage : FSM_Transistion
    {
        Unit drone;

        public ToStateEngage(Unit unit, FSM_State next): base(unit, next)
        {
            this.drone = unit;
        }

        public override bool IsValid()
        {
            return drone.HasTarget();
        }

        public override void OnTransition()
        {
        }
    }
}