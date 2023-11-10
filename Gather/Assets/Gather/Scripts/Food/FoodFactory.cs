using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class FoodFactory : MonoBehaviour
    {
        public GameObject foodPrefab;
        List<GameObject> foodPool = new List<GameObject>();

        public GameObject Spawn(Vector3 location)
        {
            if(foodPool.Count > 0)
            {
                GameObject food = foodPool.Find(f => !f.activeSelf);
                if (food)
                {
                    food.transform.position = location;
                    food.SetActive(true);
                }else
                {
                    food = Instantiate(foodPrefab, location, Quaternion.identity);
                    foodPool.Add(food);
                }
                return food;
            }
            else
            {
                GameObject food = Instantiate(foodPrefab, location, Quaternion.identity);
                foodPool.Add(food);
                return food;
            }
            
        }
    }
}
