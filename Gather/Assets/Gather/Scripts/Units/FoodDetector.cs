using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class FoodDetector : MonoBehaviour, IDetector
    {
        [SerializeField] SearchConfig config;
        List<FoodPellet> foods = new List<FoodPellet>();
        Unit unitController;
        int team;

        private void Awake()
        {
            unitController = GetComponent<Unit>();
        }

        public void SetTeam(int team)
        {
            this.team = team;
        }

        public List<FoodPellet> GetFoodList()
        {
            return foods;
        }

        public bool Detect()
        {
            TargetSystem.FindTargetsByCount<FoodPellet>(
                config.searchAmount, config.searchTag, unitController.CurrentLocation(), config.searchDist, config.searchLayer, f => f.CanTarget(team), out foods
                );
            return foods.Count > 0;
        }
    }
}