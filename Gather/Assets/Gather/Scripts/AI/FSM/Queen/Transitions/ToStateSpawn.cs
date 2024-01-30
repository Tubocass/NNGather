using gather;

namespace Gather.AI
{
    public class ToStateSpawn : FSM_Transistion
    {
        private readonly Queen queen;
        private readonly FoodManager foodCounter;

        public ToStateSpawn(Queen queen, FSM_State next): base(queen, next)
        {
            this.queen = queen;
            foodCounter = queen.GetComponent<FoodManager>();
        }

        public override bool IsValid()
        {
            return !queen.GetEnemyDetected() 
                && !foodCounter.IsFoodLow()
                && !queen.IsMoving;
        }

        public override void OnTransition()
        {
        }
    }
}