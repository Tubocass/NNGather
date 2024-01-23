using Gather.AI;
using UnityEngine;

namespace gather
{
    [RequireComponent(typeof(FarmerFSM_Controller))]
    public class FarmerDrone : Drone
    {
        FoodPellet carriedFood;
        Vector2 foodLocation = Vector2.zero;
        FoodDetector foodDetector;
        Anchor foodAnchor;

        protected override void Awake()
        {
            base.Awake();
            foodDetector = GetComponent<FoodDetector>();
            context.SetValue(Configs.FoodDetector, foodDetector);
        }

        public override void Death()
        {
            if (carriedFood)
            {
                carriedFood.Detach();
                carriedFood = null;
            }
            foodAnchor.PlaceAnchor -= SetDestination;

            base.Death();
        }

        public bool IsCarryingFood()
        {
            return carriedFood != null;
        }

        public override void SetQueen(Queen queenie)
        {
            base.SetQueen(queenie);
            foodAnchor = queenie.foodAnchor;
            foodAnchor.PlaceAnchor += SetDestination;
        }

        public override Vector2 AnchorPoint()
        {
            return foodAnchor.IsActive()? foodAnchor.GetLocation() : Location();
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
