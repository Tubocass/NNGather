using gather;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class DroneState_Return : FSM_State
    {
        Drone drone;

        public DroneState_Return(Drone drone)
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
