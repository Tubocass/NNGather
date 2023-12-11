using gather;

namespace Gather.AI
{
    public class State_Return : IBehaviorState
    {
        Drone drone;
        public State_Return(Drone drone)
        {
            this.drone = drone;
        }
        public void QueenMoved()
        {
            drone.ReturnToQueen();
        }
        public void AssesSituation()
        {
            drone.ReturnToQueen();
        }

        public void EnterState()
        {

            drone.ReturnToQueen();
        }

        public void ExitState()
        {

        }
        string IBehaviorState.ToString()
        {
            return States.returnToQueen;
        }
    }
}
