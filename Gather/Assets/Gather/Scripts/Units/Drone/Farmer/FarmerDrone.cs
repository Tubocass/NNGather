using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gather.AI;

namespace gather
{
    public class FarmerDrone : Drone
    {
        FoodPellet carriedFood;
        Vector2 foodLocation = Vector2.zero;
        [SerializeField] SearchConfig foodSearchConfig;
        bool appQuit = false;

        protected override void Awake()
        {
            base.Awake();

            context.SetValue(Configs.SearchConfig, foodSearchConfig);
            //fsmController = new FarmerFSM_Controller(this, context);
        }

        protected override void OnDisable()
        {
            if (appQuit)
            {
                return;
            }
            base.OnDisable();
            //myQueen.greenFlag -= SetDestination;
            myQueen = null;
            if (carriedFood)
            {
                carriedFood.Detach();
                carriedFood = null;
            }
            CancelInvoke();
        }
        private void OnApplicationQuit()
        {
            appQuit = true;
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

        public void SetCarriedFood(FoodPellet pellet)
        {
            this.carriedFood = pellet;
        }

        public void PickupFood(FoodPellet pellet)
        {
            // called by collider on child transform
            HaltNavigation();
            carriedFood = pellet;
            foodLocation = carriedFood.Location();
            hasTarget = false;
            fsmController.Update();
        }

        public void DropoffFood(Queen queenie)
        {
            if (IsCarryingFood() && queenie.GetTeam() == GetTeam())
            {
                HaltNavigation();
                carriedFood.Consume();
                carriedFood = null;
                queenie.Gather(foodLocation);
                fsmController.Update();
            }
        }
    }
}
