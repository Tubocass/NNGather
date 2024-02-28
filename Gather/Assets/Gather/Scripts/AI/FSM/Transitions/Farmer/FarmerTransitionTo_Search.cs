using Gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class FarmerTransitionTo_Search : FSM_Transition
    {
        private readonly FarmerDrone drone;
        bool arrivedAtSource;

        public FarmerTransitionTo_Search(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
            this.drone = context.GetValue<FarmerDrone>(Configs.Unit);
        }

        public override bool IsValid()
        {
            arrivedAtSource = context.GetValue<bool>(Configs.ArrivedAtSource);
            return (drone.IsVisitingKnownSources && arrivedAtSource) || (!drone.IsMoving);
        }
    }
}