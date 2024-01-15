using gather;

namespace Gather.AI
{
    public class ToStateSpawn : FSM_Transistion
    {
        private readonly Queen queen;
        private readonly FoodCounter counter;

        public ToStateSpawn(Queen queen, FSM_State next): base(queen, next)
        {
            this.queen = queen;
            counter = queen.GetFoodCounter();
        }

        public override bool IsValid()
        {
            return !queen.GetEnemyDetected() 
                && !counter.IsFoodLow()
                && !queen.IsMoving;
        }

        public override void OnTransition()
        {
        }
    }
}