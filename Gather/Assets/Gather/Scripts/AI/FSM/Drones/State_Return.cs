using gather;

namespace Gather.AI
{
    public class State_Return : FSM_State
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

        public override void EnterState()
        {

            drone.ReturnToQueen();
        }

        public override void Update()
        {
            drone.ReturnToQueen();
        }

        public override void ExitState()
        {

        }
        public override string GetStateName()
        {
            return States.returnToQueen;
        }
    }
}
