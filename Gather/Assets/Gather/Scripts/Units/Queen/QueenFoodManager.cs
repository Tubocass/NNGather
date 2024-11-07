using System.Collections.Generic;
using UnityEngine;

namespace Gather
{
    public class QueenFoodManager : MonoBehaviour
    {
        public int foodQueueSize = 10;
        public int foodReserve = 5;
        public int maxFood = 20;
        public int startAmount = 10;
        Queue<Vector2> foodLocations;
        MaxCounter foodCounter;

        public int Amount { get { return foodCounter.GetAmount(); } }

        protected void OnEnable()
        {
            foodCounter = ScriptableObject.CreateInstance<MaxCounter>();
            foodCounter.SetMax(maxFood);
            foodCounter.SetAmount(startAmount);
            foodLocations = new Queue<Vector2>(foodQueueSize);
        }

        public void AddAmount(int amount)
        {
            foodCounter.AddAmount(amount);
        }

        public bool CanAfford(int amount)
        {
            return Amount >= amount;
        }

        public bool IsFoodLow()
        {
            return Amount <= foodReserve;
        }

        public bool IsFoodFull()
        {
            return Amount >= maxFood;
        }

        public MaxCounter GetCounter()
        {
            return foodCounter;
        }

        public float AverageDistanceFromFood(Vector2 location)
        {
            if (foodLocations.Count == 0)
            {
                return 0f;
            } else return Vector2.Distance(location, FoodCenter());
        }

        public Vector2 FoodCenter()
        {
            if (foodLocations.Count == 0)
            {
                return Vector2.zero;
            }

            Vector2[] locations = foodLocations.ToArray();
            Vector2 avgPos = locations[0];

            for (int ap = 1; ap < locations.Length; ap++)
            {
                avgPos += locations[ap];
            }
            return avgPos /= locations.Length;
        }

        public void Gather(Vector2 fromLocation)
        {
            if (Amount < maxFood)
            {
                foodCounter.AddAmount(1);
            }

            if (!foodLocations.Contains(fromLocation))
            {
                if (foodLocations.Count < foodQueueSize)
                {
                    foodLocations.Enqueue(fromLocation);
                } else
                {
                    foodLocations.Dequeue();
                    foodLocations.Enqueue(fromLocation);
                }
            }
        }
    }
}