using gather;

namespace Gather.AI
{
    public abstract class FSM_Transistion
    {
        protected FSM_State nextState;
        protected Unit unit;

        public FSM_Transistion(Unit unit, FSM_State nextState)
        {
            this.unit = unit;
            this.nextState = nextState;
        }

        public virtual bool IsValid() { return false; }
        public virtual void OnTransition() { }
        public FSM_State GetNextState() 
        {  
            OnTransition();
            return nextState; 
        }
    }
}