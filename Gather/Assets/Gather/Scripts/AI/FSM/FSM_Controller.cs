using UnityEngine;

namespace Gather.AI
{
    public abstract class FSM_Controller : MonoBehaviour
    {
        [SerializeField] private FSM_State activeState;
        protected FSM_State initialState;
        private float timer;
        [SerializeField] private float updateTime = 0.125f;

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

        public void Start()
        {
            Init();
            ActiveState = initialState;
        }

        public void Update()
        {
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