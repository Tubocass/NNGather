using gather;
using Gather.AI.FSM.States;
using UnityEngine;

namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_LookForFood : FSM_Transition
    {
        FarmerDrone drone;
        public To_DroneState_LookForFood(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
            drone = context.GetValue<FarmerDrone>(Configs.Unit);
        }

        public override bool IsValid()
        {
            return drone.IsSearchingForFood;
        }
    }
}