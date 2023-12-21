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
        Blackboard context;
        int team;
        bool avoidDogPile;
        
        public State_Search(FarmerDrone drone, Blackboard context)
        {
            this.context = context;
            this.drone = drone;
            config = context.GetValue<SearchConfig>(Configs.SearchConfig);
        }

        public override void EnterState()
        {
            team = drone.GetTeam();
            avoidDogPile = true;
        }

        public override void Update()
        {
            Search();
        }

        public override void ExitState()
        {
            foodPellets.Clear();
        }

        public override string GetStateName()
        {
            return States.search;
        }

        void Search()
        {
            TargetSystem.FindTargetsByCount(config.searchAmount, config.searchTag, drone.Location(), config.searchDist, config.searchLayer, f => f.CanTarget(team), out foodPellets) ;

            if (foodPellets.Count > 0)
            {
                targetFood = TargetSystem.TargetNearest(drone.Location(), foodPellets);
                context.SetValue<ITarget>(Configs.Target, targetFood);
                drone.hasTarget = true;
            }
            else if (!drone.IsMoving || avoidDogPile)
            {
                avoidDogPile = false;
                drone.MoveRandomly();
            }
        }
    }
}
