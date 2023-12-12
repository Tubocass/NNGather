using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public abstract class FSM_Controller
    {
        public IBehaviorState BehaviorState
        {
            get { return behaviorState; }
            set
            {
                if (behaviorState != null)
                {
                    behaviorState.ExitState();
                }
                behaviorState = value;
                behaviorState.EnterState();
            }
        }
        private IBehaviorState behaviorState;

        public virtual void Disable()
        {
            if (BehaviorState != null)
            {
                BehaviorState.ExitState();
            }
        }
    }
}