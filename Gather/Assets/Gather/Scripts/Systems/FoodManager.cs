using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class FoodManager
    {
        Dictionary<int, int> foodTargets = new Dictionary<int, int>();
        List<Vector2> knownFoodSources = new List<Vector2>();

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

        public void AddFoodSource(Vector2 foodSource)
        {
            if (!knownFoodSources.Contains(foodSource))
                knownFoodSources.Add(foodSource);
        }

        public List<Vector2> GetFoodSources() {  return knownFoodSources; }

        public Vector2 NearsestFoodSourceLocation(Vector2 location)
        {
            return TargetSystem.TargetNearest(location, knownFoodSources);
        }
    }
}