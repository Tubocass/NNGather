using gather;
using Gather.AI.FSM.States;


namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_CheckForKnownFood : FSM_Transition
    {
        FarmerDrone drone;
        public To_DroneState_CheckForKnownFood(FarmerDrone drone, FSM_State nextState) : base(drone, nextState)
        {
            this.drone = drone;
        }

        public override bool IsValid()
        {
            return drone.sourcesToVist.Count == 0 && drone.TeamConfig.FoodManager.GetFoodSources().Count > 1;
        }
    }
}