using System.Collections.Generic;

namespace gather
{
    public class FoodManager
    {
        Dictionary<int, int> foodTargets = new Dictionary<int, int>();

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
    }
}