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
        [SerializeField] SearchConfig enemySearchConfig;
        bool appQuit = false;
        EnemyDetector enemyDetector;
        List<FighterDrone> enemies = new List<FighterDrone>();
        Blackboard context = new Blackboard();


        protected override void Awake()
        {
            base.Awake();
            context.SetValue(Configs.SearchConfig, foodSearchConfig);
            context.SetValue(Configs.EnemySearchConfig, enemySearchConfig);
            enemyDetector = GetComponentInChildren<EnemyDetector>();
            AIController = new FarmerFSM_Controller(this, enemyDetector, context);
        }

        public override void SetTeam(TeamConfig config)
        {
            base.SetTeam(config);
            Enable();
        }

        private void Enable()
        {
            AIController.Enable(GetTeam());
        }

        protected override void OnDisable()
        {
            if (appQuit)
            {
                return;
            }
            base.OnDisable();
            teamConfig.SetUnitCount(TeamConfig.UnitType.Farmer, -1);
            myQueen.greenFlag -= SetDestination;
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
            queenie.greenFlag += SetDestination;
        }

        public void SetCarriedFood(FoodPellet pellet)
        {
            this.carriedFood = pellet;
        }

        public void PickupFood(FoodPellet pellet)
        {
            // called by collider on child transform
            carriedFood = pellet;
            foodLocation = carriedFood.Location();
            HaltNavigation();
            AIController.AssessSituation();
        }

        public void DropoffFood(Queen queenie)
        {
            if (IsCarryingFood() && queenie.GetTeam() == GetTeam())
            {
                HaltNavigation();
                carriedFood.Consume();
                carriedFood = null;
                queenie.Gather(foodLocation);
                AIController.AssessSituation();
            }
        }
    }
}
