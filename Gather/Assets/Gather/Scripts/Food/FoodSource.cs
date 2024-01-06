using System.Collections;
using UnityEngine;

namespace gather
{
    public class FoodSource : MonoBehaviour
    {
        Vector2[] spawnPositions;
        FoodFactory foodFactory;
        [SerializeField] bool isTimeVariable;
        [SerializeField] float timer;
        [SerializeField] float variableTimeAmount;
        [Space]
        [SerializeField] int spawnRangeDist;
        [SerializeField] float minimumDistance;
        [Space]
        [SerializeField] int baseFood;
        [SerializeField] int variableFood;
        Vector2 center { get => (Vector2)transform.position; }

        /*
         *  Would like to see these random variables under a Normal curve
         */

        private void Awake()
        {
            foodFactory = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<FoodFactory>();
            int numPositions = Random.Range(1, variableFood) + baseFood;
            spawnPositions = new Vector2[numPositions];
            Vector2 pos = Vector2.zero;

            for (int p = 0; p < spawnPositions.Length; p++)
            {
                do
                {
                    pos = Random.insideUnitCircle * spawnRangeDist;
                } while (!MinimumDistanceCheck(pos, p));
                
                spawnPositions[p] = pos;
            }

            if (isTimeVariable)
            {
                float varTime = Random.Range(-variableTimeAmount, variableTimeAmount);
                timer += varTime;
            }
            transform.Rotate(Vector3.forward, Random.value * 360);
            SpawnAllAtOnce();
            StartCoroutine(Respawn());
        }

        IEnumerator Respawn()
        {
            while (Application.isPlaying)
            {
                for (int p = 0; p < spawnPositions.Length; p++)
                {
                    yield return new WaitForSeconds(timer);

                    if (!Physics2D.OverlapPoint(center + spawnPositions[p], MaskLayers.food))
                    {
                        foodFactory.Spawn(center + spawnPositions[p])
                            .transform.SetParent(this.transform);
                    }
                }
            }
        }

        void SpawnAllAtOnce()
        {
            for (int p = 0; p < spawnPositions.Length; p++)
            {
                foodFactory.Spawn(center + spawnPositions[p])
                       .transform.SetParent(this.transform);
            }
        }

        bool MinimumDistanceCheck(Vector2 point, int previousAmount)
        {
            if (previousAmount == 0)
            {
                return true;
            }
            for (int p = 0; p < previousAmount; p++)
            {
                if (Vector2.Distance(point, spawnPositions[p]) < minimumDistance)
                {
                    return false;
                }
            }
            return true;
        }
    }
}