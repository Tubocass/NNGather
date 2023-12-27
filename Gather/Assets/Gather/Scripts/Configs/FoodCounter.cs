using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace gather
{
    [CreateAssetMenu]
    public class FoodCounter : Counter
    {
        public int foodQueueSize = 10;
        public int foodReserve = 5;
        public int maxFood = 20;
        Queue<Vector2> foodLocations;
        public int FoodCount { get { return amount; } }

        protected override void OnEnable()
        {
            foodLocations = new Queue<Vector2>(foodQueueSize);
            defaultAmount = 10;
            base.OnEnable();
        }

        public bool IsFoodLow()
        {
            return amount <= foodReserve;
        }

        public bool IsFoodFull()
        {
            return amount >= maxFood;
        }

        public float AverageDistanceFromFood(Vector2 location)
        {
            if (foodLocations.Count == 0) 
            { 
                return 0f; 
            }else return Vector2.Distance(location, FoodCenter());
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
            if(amount < maxFood)
            {
                AddAmount(1);
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
