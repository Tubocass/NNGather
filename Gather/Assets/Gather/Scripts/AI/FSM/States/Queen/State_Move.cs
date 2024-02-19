using gather;

namespace Gather.AI.FSM.States
{
    public class State_Move : FSM_State
    {
        Queen queen;
        FoodManager foodCounter;

        public State_Move(Queen queen)
        {
            this.queen = queen;
            foodCounter = queen.GetComponent<FoodManager>();
        }

        public override void EnterState()
        {
            MoveToFoodCenter();
        }

        void MoveToFoodCenter()
        {
            queen.SetDestination(foodCounter.FoodCenter());
        }

        public override void Update()
        {
         
        }

        public override void ExitState()
        {
        }
    }
}