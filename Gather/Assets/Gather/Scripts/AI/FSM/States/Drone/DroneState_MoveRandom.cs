using gather;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class DroneState_MoveRandom: FSM_State
    {
        FarmerDrone drone;

        public DroneState_MoveRandom(Blackboard context) : base(context)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
        }

        public override void EnterState()
        {
            Debug.Log("MoveRandom");
            drone.MoveRandomly(drone.AnchorPoint());
        }

    }
}