using System.Collections.Generic;

namespace Gather.AI
{
    public abstract class FSM_State 
    {
        public List<FSM_Transistion> transistions = new List<FSM_Transistion>();

        public void AddTransitions(params FSM_Transistion[] transistions)
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