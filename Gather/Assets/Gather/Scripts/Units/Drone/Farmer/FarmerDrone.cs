using Gather.AI.FSM.Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    [RequireComponent(typeof(FarmerFSM_Controller))]
    public class FarmerDrone : Drone
    {
        FoodBerry carriedFood;
        Vector2 foodLocation = Vector2.zero;
        FoodDetector foodDetector;
        Anchor foodAnchor;

        public Queue<Vector2> waypoints = new Queue<Vector2>();
        public bool isExploring = false;

        public bool IsSearchingForFood { get { return !GetEnemyDetected() && !IsCarryingFood() && !context.GetValue<bool>(Configs.HasTarget); } }
        public bool IsVisitingKnownSources { get { return waypoints.Count > 0; } }
        public bool CanCheckForSources { get { return !isExploring && waypoints.Count == 0 && TeamConfig.FoodManager.GetFoodSources().Count > 0; } }

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
            if (context.GetValue<bool>(Configs.HasTarget))
            {
                ITargetable target = context.GetValue<ITargetable>(Configs.Target);
                UntargetFood((FoodBerry)target);
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

        public bool CanTargetFood(FoodBerry food)
        {
            return teamConfig.FoodManager.CanTargetFood(GetInstanceID(), food.GetInstanceID());
        }

        public void TargetFood(FoodBerry food)
        {
            teamConfig.FoodManager.TargetFood(GetInstanceID(), food.GetInstanceID());
            context.SetValue<ITargetable>(Configs.Target, food);
            SetHasTarget(true);
        }

        public void UntargetFood(FoodBerry food)
        {
            context.SetValue<ITargetable>(Configs.Target, null);
            teamConfig.FoodManager.UntargetFood(food.GetInstanceID());
        }

        public override void SetHasTarget(bool hasTarget)
        {
            base.SetHasTarget(hasTarget);
            if (!hasTarget)
            {
                FoodBerry food = (FoodBerry)context.GetValue<ITargetable>(Configs.Target);
                if(food != null)
                {
                    UntargetFood(food);
                }
            }
        }

        public void PickupFood(FoodBerry pellet)
        {
            // called by collider on child transform
            HaltNavigation();
            carriedFood = pellet;
            foodLocation = carriedFood.ParentBush.GetLocation();
            SetHasTarget(false);
            UntargetFood(pellet);
        }

        public void DropoffFood(Queen queenie)
        {
            if (IsCarryingFood() && queenie.GetTeamID() == GetTeamID())
            {
                HaltNavigation();
                carriedFood.Consume();
                carriedFood = null;
                queenie.Gather(foodLocation);
                isExploring = false;
                waypoints.Clear();
            }
        }
    }
}
