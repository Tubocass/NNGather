namespace Gather.AI
{
    public abstract class FSM_Controller
    {
        private FSM_State activeState;
        protected FSM_State initialState;
        bool enabled;

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

        public virtual void Enable()
        {
            ActiveState = initialState;
            enabled = true;
        }

        public void Update()
        {
            if(!enabled)
            {
                Enable();
            }
            foreach(FSM_Transistion transistion in ActiveState.transistions)
            {
                if(transistion.isValid())
                {
                    ActiveState = transistion.GetNextState();
                }
            }
            ActiveState.Update();

        }

        public virtual void Disable()
        {
            if (ActiveState != null)
            {
                ActiveState.ExitState();
            }
            enabled = false;
        }
    }
}