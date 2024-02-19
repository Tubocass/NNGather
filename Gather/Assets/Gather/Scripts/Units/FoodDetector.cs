using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class FoodDetector : MonoBehaviour
    {
        [SerializeField] SearchConfig config;
        List<FoodPellet> foods = new List<FoodPellet>();
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

        public List<FoodPellet> GetFoodList()
        {
            return foods;
        }

        public void Detect()
        {
            TargetSystem.FindTargetsByCount<FoodPellet>(
                config.searchAmount, config.searchTag, unitController.GetLocation(), config.searchDist, config.searchLayer, f => f.CanBeTargeted(team) && unitController.CanTargetFood(f), out foods
                );
            detectedSomething =  foods.Count > 0;
        }
    }
}