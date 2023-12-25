using gather;

namespace Gather.AI
{
    public class ToStateFlee : FSM_Transistion
    {
        Unit unit;
        FSM_State nextState;
        public ToStateFlee(Unit unit, FSM_State next)
        {
            this.unit = unit;
            this.nextState = next;
        }

        public bool isValid()
        {
            return unit.GetEnemyDetected();
        }

        public FSM_State GetNextState()
        {
            OnTransition();
            return nextState;
        }

        public void OnTransition()
        {
        }
    }
}