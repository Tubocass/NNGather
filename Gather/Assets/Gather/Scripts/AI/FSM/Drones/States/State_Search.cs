using System.Collections.Generic;
using gather;
using UnityEngine;

namespace Gather.AI
{
    public class State_Search : FSM_State
    {
        List<FoodPellet> foodPellets = new List<FoodPellet>();
        FarmerDrone drone;
        FoodPellet targetFood;
        SearchConfig config;
        int team;
        bool avoidDogPile;
        
        public State_Search(FarmerDrone drone, Blackboard context)
        {
            this.drone = drone;
            this.config = context.GetValue<SearchConfig>(Configs.SearchConfig);
        }

        public override void EnterState()
        {
            team = drone.GetTeam();
            Search();

        }

        public override void Update()
        {
            if (targetFood && !targetFood.CanTarget(team))
            {
                //targetFood.UnTargeted(drone);
                targetFood = null;
                avoidDogPile = true;
            }
            if (!targetFood)
            {
                Search();
            }
        }

        public override void ExitState()
        {
            Clear();
        }

        public override string GetStateName()
        {
            return States.search;
        }

        public void Clear()
        {
            foodPellets.Clear();
            if (targetFood)
            {
                targetFood = null;
            }
        }

        void Search()
        {
            TargetSystem.FindTargetsByCount<FoodPellet>(config.searchAmount, config.searchTag, drone.Location(), config.searchDist, config.searchLayer, f => f.CanTarget(team), out foodPellets) ;

            if (foodPellets.Count > 0)
            {
                targetFood = TargetSystem.TargetNearest<FoodPellet>(drone.Location(), foodPellets);
                drone.SetDestination(targetFood.Location());
                //targetFood.Targeted(drone);
            }
            else if (!drone.IsMoving || avoidDogPile)
            {
                avoidDogPile = false;
                drone.MoveRandomly();
            }
        }
    }
}
