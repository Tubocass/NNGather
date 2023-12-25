using gather;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace Gather.AI
{
    public class ToStateFeed : FSM_Transistion
    {
        Queen queen;
        FSM_State nextState;
        FoodCounter counter;

        public ToStateFeed(Queen queen, FSM_State next)
        {
            this.queen = queen;
            this.nextState = next;
            counter = queen.GetBlackboard().GetValue<FoodCounter>(Configs.FoodCounter);
        }

        public bool isValid()
        {
            return !queen.GetEnemyDetected() 
                && counter.IsFoodLow() 
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