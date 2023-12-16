using System.Collections;
using UnityEngine;

namespace gather
{
    public class FoodSource : MonoBehaviour
    {
        Vector3[] spawnPositions;
        FoodFactory foodFactory;
        [SerializeField] int range = 4;
        [SerializeField] float timer = 5f;
        [SerializeField] bool variableTime;
        [SerializeField] float variableTimeAmount = 1f;

        private void Awake()
        {
            foodFactory = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<FoodFactory>();
            int numPositions = Random.Range(1, 4) + 3;
            spawnPositions = new Vector3[numPositions];
            for (int p = 0; p < spawnPositions.Length; p++)
            {
                int x = (int)Mathf.Round(Random.Range(-range, range));
                int y = (int)Mathf.Round(Random.Range(-range, range));
                spawnPositions[p] = new Vector3(x,y,0);
            }
            if(variableTime)
            {
                float varTime = Random.Range(-variableTimeAmount, variableTimeAmount);
                timer += varTime;
            }
            InvokeRepeating("Respawn", 0f, timer);
        }
        void Respawn()
        {
            for (int p = 0; p < spawnPositions.Length; p++)
            {
                if(!Physics2D.OverlapPoint(transform.position + spawnPositions[p], MaskLayers.food))
                {
                    foodFactory.Spawn(transform.position + spawnPositions[p])
                        .transform.SetParent(this.transform);
                }
            }
        }
    }
}