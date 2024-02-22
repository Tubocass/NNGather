using Gather.AI.FSM.States;
using Gather.AI.FSM.Transitions;
using System.Collections.Generic;
using UnityEngine;

namespace Gather.AI.FSM.Controllers
{
    public abstract class FSM_Controller : MonoBehaviour
    {
        protected List<FSM_State> states = new List<FSM_State>();
        protected FSM_State initialState;
        private FSM_State activeState;
        private bool isEnabled = false;

        public FSM_State ActiveState
        {
            get { return activeState; }
            set
            {
                if (activeState != null)
                {
                    activeState.ExitState();
                }
                activeState = value;
                activeState.EnterState();
            }
        }

        public void Update()
        {
            if (!isEnabled)
            {
                Init();
                ActiveState = initialState;
                isEnabled = true;
            }

            Tick();
        }

        protected virtual void Init()
        {

        }

        public virtual void Tick()
        {
            foreach (FSM_Transition transistion in ActiveState.transistions)
            {
                if (transistion.IsValid())
                {
                    ActiveState = transistion.GetNextState();
                }
            }
            ActiveState.Update();
        }

        public virtual void OnDisable()
        {
            if (ActiveState != null)
            {
                ActiveState.ExitState();
            }
        }
    }
}