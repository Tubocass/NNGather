using System.Collections.Generic;
using gather;

namespace Gather.AI
{
    public class State_Search : IBehaviorState
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

        public void EnterState()
        {
            team = drone.GetTeam();
            Search();

        }

        public void AssesSituation()
        {
            if (targetFood)
            {
                if (!targetFood.CanTarget(team))
                {
                    targetFood.Targeted(team, false);
                    targetFood = null;
                }else
                {
                    // this should technically only hit if the food was moved by god myself
                    drone.SetDestination(targetFood.Location());
                }
            }
            if (!drone.IsCarryingFood() && !targetFood)
            {
                Search();
            }
        }

        public void ExitState()
        {
            Clear();
        }

        string IBehaviorState.ToString()
        {
            return States.search;
        }

        public void Clear()
        {
            foodPellets.Clear();
            if (targetFood)
            {
                targetFood.Targeted(team, false);
                targetFood = null;
            }

        }

        void Search()
        {
            foodPellets = TargetSystem.FindTargetsByCount<FoodPellet>(config.searchAmount, config.searchTag, drone.Location(), config.searchDist, config.searchLayer, f => f.CanTarget(team)) ;

            if (foodPellets.Count > 0)
            {
                targetFood = TargetSystem.TargetNearest<FoodPellet>(drone.Location(), foodPellets);
                targetFood.Targeted(team, true);
                drone.SetDestination(targetFood.Location());
            }
            else
            {
                drone.MoveRandomly();
            }
        }
    }
}