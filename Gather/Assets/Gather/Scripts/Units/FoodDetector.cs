using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class FoodDetector : MonoBehaviour
    {
        public SearchConfig config;
        List<FoodBerry> foods = new List<FoodBerry>();
        FarmerDrone unitController;
        int team;
        bool detectedSomething = false;
        public bool DetectedSomething => detectedSomething;

        private void Awake()
        {
            unitController = GetComponent<FarmerDrone>();
        }

        public void SetTeam(int team)
        {
            this.team = team;
        }

        public List<FoodBerry> GetFoodList()
        {
            return foods;
        }

        public void Detect()
        {
            foods = TargetSystem.FindTargetsByCount<FoodBerry>(
                config.searchAmount, unitController.GetLocation(), config.searchDist, config.searchLayer, f => f.CanBeTargeted(team) && unitController.CanTargetFood(f));
            detectedSomething = foods != null && foods.Count > 0;
        }
    }
}