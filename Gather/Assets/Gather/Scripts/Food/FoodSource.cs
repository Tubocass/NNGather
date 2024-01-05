using System.Collections;
using System;
using UnityEngine;

namespace gather
{
    public class FoodSource : MonoBehaviour
    {
        Vector3[] spawnPositions;
        FoodFactory foodFactory;
        [SerializeField] int spawnRangeDist = 4;
        [SerializeField] float timer = 5f;
        [SerializeField] bool isTimeVariable;
        [SerializeField] float variableTimeAmount = 1f;
        [SerializeField] int baseFood, variableFood;

        /*
         *  Would like to see these random variables under a Normal curve
         */

        private void Awake()
        {
            foodFactory = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<FoodFactory>();
            int numPositions = UnityEngine.Random.Range(1, variableFood) + baseFood;
            spawnPositions = new Vector3[numPositions];

            for (int p = 0; p < spawnPositions.Length; p++)
            {
                Vector2 pos = UnityEngine.Random.insideUnitCircle * spawnRangeDist;
                spawnPositions[p] = pos;
            }
            if(isTimeVariable)
            {
                float varTime = UnityEngine.Random.Range(-variableTimeAmount, variableTimeAmount);
                timer += varTime;
            }
            SpawnAllAtOnce();
            StartCoroutine(Respawn());
        }

        IEnumerator Respawn()
        {
            for (int p = 0; p < spawnPositions.Length; p++)
            {
                yield return new WaitForSeconds(timer);

                if (!Physics2D.OverlapPoint(transform.position + spawnPositions[p], MaskLayers.food))
                {
                    foodFactory.Spawn(transform.position + spawnPositions[p])
                        .transform.SetParent(this.transform);
                }
            }
        }

        void SpawnAllAtOnce()
        {
            for (int p = 0; p < spawnPositions.Length; p++)
            {
                foodFactory.Spawn(transform.position + spawnPositions[p])
                       .transform.SetParent(this.transform);
            }
        }
    }
}