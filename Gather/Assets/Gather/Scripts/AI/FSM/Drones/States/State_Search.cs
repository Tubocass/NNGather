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
                targetFood = null;
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
            foodPellets = TargetSystem.FindTargetsByCount<FoodPellet>(config.searchAmount, config.searchTag, drone.Location(), config.searchDist, config.searchLayer, f => f.CanTarget(team)) ;

            if (foodPellets.Count > 0)
            {
                targetFood = TargetSystem.TargetNearest<FoodPellet>(drone.Location(), foodPellets);
                drone.SetDestination(targetFood.Location());
            }
            else
            {
                if(!drone.IsMoving)
                {
                    drone.MoveRandomly();
                }
            }
        }
    }
}
