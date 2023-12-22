using UnityEngine;

namespace gather
{
    public class FarmerDrone : Drone
    {
        FoodPellet carriedFood;
        Vector2 foodLocation = Vector2.zero;
        [SerializeField] SearchConfig foodSearchConfig;

        protected override void Awake()
        {
            base.Awake();

            context.SetValue(Configs.SearchConfig, foodSearchConfig);
        }

        public override void Death()
        {
            if (carriedFood)
            {
                carriedFood.Detach();
                carriedFood = null;
            }
            //    //myQueen.greenFlag -= SetDestination;

            base.Death();
        }

        public bool IsCarryingFood()
        {
            return carriedFood != null;
        }

        public override void SetQueen(Queen queenie)
        {
            base.SetQueen(queenie);
            //queenie.greenFlag += SetDestination;
        }

        public void PickupFood(FoodPellet pellet)
        {
            // called by collider on child transform
            HaltNavigation();
            carriedFood = pellet;
            foodLocation = carriedFood.Location();
            hasTarget = false;
            fsmController.Tick();
        }

        public void DropoffFood(Queen queenie)
        {
            if (IsCarryingFood() && queenie.GetTeam() == GetTeam())
            {
                //HaltNavigation();
                carriedFood.Consume();
                carriedFood = null;
                queenie.Gather(foodLocation);
                fsmController.Tick();
            }
        }
    }
}
