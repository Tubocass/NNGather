using gather;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class DroneState_Return : FSM_State
    {
        Drone drone;

        public DroneState_Return(Blackboard context) : base(context)
        {
            this.drone = context.GetValue<Drone>(Configs.Unit);
        }

        public override void EnterState()
        {
            Debug.Log("Return");
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
