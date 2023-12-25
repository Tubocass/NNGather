using gather;

namespace Gather.AI
{
    public class State_Return : FSM_State
    {
        Drone drone;
        public State_Return(FarmerDrone drone, Blackboard context)
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
