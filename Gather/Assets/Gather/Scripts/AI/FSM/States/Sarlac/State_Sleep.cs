using UnityEngine;

namespace Gather.AI.FSM.States
{

    public class State_Sleep : FSM_State
    {
        public override void EnterState()
        {
            base.EnterState();
            Debug.Log("Asleep");
        }

    }
}
