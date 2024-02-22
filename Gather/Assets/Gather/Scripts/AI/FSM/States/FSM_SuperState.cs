using Gather.AI.FSM.Transitions;
using System.Collections.Generic;

namespace Gather.AI.FSM.States
{
    public abstract class FSM_SuperState : FSM_State
    {
        protected List<FSM_State> subStates;
        protected FSM_State initialState;
        private FSM_State lastState;
        bool isEnabled = false;

        protected FSM_SuperState(Blackboard context) : base(context)
        {
        }

        public FSM_State LastState
        {
            get { return lastState; }
            set
            {
                if (lastState != null)
                {
                    lastState.ExitState();
                }
                lastState = value;
                lastState.EnterState();
            }
        }

        public override void EnterState()
        {
            if (!isEnabled)
            {
                Init();
                LastState = initialState;
                isEnabled = true;
            }
            LastState.EnterState();
        }

        public virtual void Init()
        {
            
        }

        public override void Update()
        {
            foreach (FSM_Transition transistion in lastState.transistions)
            {
                if (transistion.IsValid())
                {
                    LastState = transistion.GetNextState();
                }
            }
            LastState.Update();
        }

        public override void ExitState()
        {
            LastState.ExitState();
        }
    }
}