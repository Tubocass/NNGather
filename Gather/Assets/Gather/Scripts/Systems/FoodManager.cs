using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class FoodManager
    {
        Dictionary<int, int> foodTargets = new Dictionary<int, int>();
        List<FoodSource> knownFoodSources = new List<FoodSource>();

        public void TargetFood(int droneID, int foodID)
        {
            if (!foodTargets.ContainsKey(foodID))
            {
                foodTargets.Add(foodID, droneID);
            }
        }

        public bool CanTargetFood(int droneID, int foodID)
        {
            int heldID;
            if (foodTargets.TryGetValue(foodID, out heldID))
            {
                return heldID == droneID;
            } else return true;
        }

        public void UntargetFood(int foodID)
        {
            if (foodTargets.ContainsKey(foodID))
            {
                foodTargets.Remove(foodID);
            }
        }

        public void AddFoodSource(FoodSource foodSource)
        {
            knownFoodSources.Add(foodSource);
        }

        public Vector2 NearsestFoodSourceLocation(Vector2 location)
        {
            if(knownFoodSources.Count > 0)
            {
                return TargetSystem.TargetNearest(location, knownFoodSources).transform.position;
            }else return Vector2.zero;
        }
    }
}