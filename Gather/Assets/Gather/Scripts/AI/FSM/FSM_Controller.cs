using UnityEngine;

namespace Gather.AI
{
    public abstract class FSM_Controller : MonoBehaviour
    {
        private FSM_State activeState;
        protected FSM_State initialState;
        [SerializeField] private float updateTime = 0.125f;
        private float timer;
        private bool isEnabled;

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

        protected virtual void Init()
        {

        }

        public void Enable()
        {
            Init();
            ActiveState = initialState;
        }

        public void Update()
        {
            if(!isEnabled)
            {
                Enable();
                isEnabled = true;
            }
            timer += Time.deltaTime;
            if (timer >= updateTime)
            {
                timer = 0;
                Tick();
            }
        }

        public virtual void Tick()
        { 
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
        }
    }
}