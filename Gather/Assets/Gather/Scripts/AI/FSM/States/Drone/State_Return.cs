using gather;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class State_Return : FSM_State
    {
        Drone drone;

        public State_Return(Drone drone)
        {
            this.drone = drone;
        }

        public override void EnterState()
        {
        }

        public override void Update()
        {
            drone.ReturnToQueen();   
        }

        public override void ExitState()
        {
        }
    }
}
