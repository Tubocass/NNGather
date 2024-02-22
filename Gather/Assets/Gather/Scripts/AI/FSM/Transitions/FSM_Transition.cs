using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public abstract class FSM_Transition
    {
        protected FSM_State nextState;
        protected Unit unit;
        protected Blackboard context;

        public FSM_Transition(Blackboard context, FSM_State nextState)
        {
            this.context = context;
            this.unit = context.GetValue<Unit>(Configs.Unit);
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