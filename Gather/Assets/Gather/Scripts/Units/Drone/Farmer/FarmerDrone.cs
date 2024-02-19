using Gather.AI.FSM.Controllers;
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
            if (HasTarget)
            {
                ITargetable target = context.GetValue<ITargetable>(Configs.Target);
                UntargetFood((FoodPellet)target);
            }
            base.Death();
        }

        public override void SetQueen(Queen queenie)
        {
            base.SetQueen(queenie);
            foodAnchor = queenie.foodAnchor;
            foodAnchor.PlaceAnchor += SetDestination;
        }

        public override Vector2 AnchorPoint()
        {
            return foodAnchor.IsActive()? foodAnchor.GetLocation() : GetLocation();
        }
        public bool IsCarryingFood()
        {
            return carriedFood != null;
        }

        public bool CanTargetFood(FoodPellet food)
        {
            return teamConfig.FoodManager.CanTargetFood(GetInstanceID(), food.GetInstanceID());
        }

        public void TargetFood(FoodPellet food)
        {
            teamConfig.FoodManager.TargetFood(GetInstanceID(), food.GetInstanceID());
        }

        public void UntargetFood(FoodPellet food)
        {
            teamConfig.FoodManager.UntargetFood(food.GetInstanceID());
        }

        public void PickupFood(FoodPellet pellet)
        {
            // called by collider on child transform
            HaltNavigation();
            carriedFood = pellet;
            foodLocation = carriedFood.GetLocation();
            SetHasTarget(false);
            UntargetFood(pellet);
        }

        public void DropoffFood(Queen queenie)
        {
            if (IsCarryingFood() && queenie.GetTeam() == GetTeam())
            {
                //HaltNavigation();
                carriedFood.Consume();
                carriedFood = null;
                queenie.Gather(foodLocation);
            }
        }
    }
}
