using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class FarmerDrone : Drone
    {
        FoodPellet carriedFood;
        Vector2 foodLocation = Vector2.zero;
        [SerializeField] SearchConfig foodSearchConfig;
        [SerializeField] SearchConfig enemySearchConfig;
        State_Search searchState;
        State_Flee fleeState;
        bool appQuit = false;
        EnemyDetector enemyDetector;
        List<FighterDrone> enemies = new List<FighterDrone>();

        protected override void Awake()
        {
            base.Awake();
            enemyDetector = GetComponentInChildren<EnemyDetector>();
            enemyDetector.SetEnemyType(unit => unit.GetType() == typeof(FighterDrone));
            enemyDetector.EnemyDetected += Flee;
            enemyDetector.AllClear += Idle;
            searchState = new State_Search(this, foodSearchConfig);
            fleeState = new State_Flee(this, enemySearchConfig);
        }

        public override void SetTeam(TeamConfig config)
        {
            base.SetTeam(config);
            Enable();
        }

        private void Enable()
        {
            enemyDetector.SetTeam(GetTeam());
            SearchForFood();
        }

        protected override void OnDisable()
        {
            if(appQuit)
            {
                return;
            }
            base.OnDisable();
            teamConfig.SetUnitCount(TeamConfig.UnitType.Farmer, -1);
            myQueen.QueenMove -= QueenMoved;
            myQueen.greenFlag -= SetDestination;
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

        void Idle()
        {
            if (IsCarryingFood())
            {
                if (BehaviorState.ToString() != States.returnToQueen)
                {
                    BehaviorState = returnState;
                }
            }
            else if (BehaviorState.ToString() != States.search)
            {
                SearchForFood();
            }
        }
        
        void SearchForFood()
        {
            BehaviorState = searchState;
        }

        public void Flee()
        {
            //HaltNavigation();
            //fleeState.SetEnemiesList(enemyDetector.GetEnemiesList());
            BehaviorState = fleeState;
        }

        protected override void QueenMoved()
        {
            base.QueenMoved();
            if(BehaviorState.ToString() == States.returnToQueen)
            {
                BehaviorState.AssesSituation();
            }
        }

        public override void SetQueen(Queen queenie)
        {
            base.SetQueen(queenie);
            queenie.QueenMove += QueenMoved;
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
            BehaviorState = returnState;
        }

        public void DropoffFood(Queen queenie)
        {
            if (IsCarryingFood() && queenie.GetTeam() == GetTeam())
            {
                HaltNavigation();
                carriedFood.Consume();
                carriedFood = null;
                queenie.Gather(foodLocation);
                SearchForFood();
            }
        }
    }
}
