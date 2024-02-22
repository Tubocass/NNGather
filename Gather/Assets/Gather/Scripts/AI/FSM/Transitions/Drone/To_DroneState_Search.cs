using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_DroneState_Search : FSM_Transition
    {
        private readonly FarmerDrone drone;
        bool arrivedAtSource;

        public To_DroneState_Search(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
        }

        public override bool IsValid()
        {
            arrivedAtSource = context.GetValue<bool>("arrivedAtSource");
            return (drone.IsVisitingKnownSources && arrivedAtSource) || (!drone.IsMoving && drone.IsSearchingForFood);
        }

        public override void OnTransition()
        {
            //Debug.Log("Start search");
        }
    }
}