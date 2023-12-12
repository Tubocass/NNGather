namespace Gather.AI
{
    public abstract class FSM_Controller
    {
        public FSM_State ActiveState
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
        private FSM_State behaviorState;

        public virtual void Disable()
        {
            if (ActiveState != null)
            {
                ActiveState.ExitState();
            }
        }
    }
}