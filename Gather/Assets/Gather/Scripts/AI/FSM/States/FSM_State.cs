using Gather.AI.FSM.Transitions;
using System.Collections.Generic;

namespace Gather.AI.FSM.States
{
    public abstract class FSM_State
    {
        public List<FSM_Transition> transistions = new List<FSM_Transition>();
        protected Blackboard context;

        public FSM_State(Blackboard context) 
        {
            this.context = context;
        }

        public void AddTransitions(params FSM_Transition[] transistions)
        {
            foreach (var trans in transistions)
            {
                this.transistions.Add(trans);
            }
        }

        public virtual void EnterState() { }
        public virtual void Update() { }
        public virtual void ExitState() { }
    }
}