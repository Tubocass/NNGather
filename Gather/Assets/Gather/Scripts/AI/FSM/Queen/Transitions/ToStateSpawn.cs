using gather;

namespace Gather.AI
{
    public class ToStateSpawn : FSM_Transistion
    {
        Queen queen;
        FSM_State nextState;
        FoodCounter counter;

        public ToStateSpawn(Queen queen, FSM_State next)
        {
            this.queen = queen;
            this.nextState = next;
            counter = queen.GetBlackboard().GetValue<FoodCounter>(Configs.FoodCounter);
        }

        public bool isValid()
        {
            return !queen.GetEnemyDetected() 
                && !counter.IsFoodLow()
                && !queen.IsMoving;
        }

        public FSM_State GetNextState()
        {
            return nextState;
        }

        public void OnTransition()
        {
        }
    }
}