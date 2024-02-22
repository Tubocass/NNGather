using UnityEngine;

namespace Gather.AI.FSM.States
{

    public class SarlacState_Sleep : FSM_State
    {
        public SarlacState_Sleep(Blackboard context) : base(context)
        {
        }

        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Asleep");
        }

    }
}
