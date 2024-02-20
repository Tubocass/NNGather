using System.Collections;
using UnityEngine;

namespace gather
{
    public class FoodBush : MonoBehaviour, ITargetable
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
        Transform myTransform;

        /*
         *  Would like to see these random variables under a Normal curve
         */

        private void Awake()
        {
            myTransform = transform;
            foodFactory = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<FoodFactory>();

            SetupSpawnPoints();
            transform.Rotate(Vector3.forward, Random.value * 360);
            SpawnAllAtOnce();
            StartCoroutine(Respawn());
        }

        void SetupSpawnPoints()
        {
            int numPositions = Random.Range(1, variableFood) + baseFood;
            spawnPositions = new Vector2[numPositions];
            Vector2 pos;

            for (int p = 0; p < spawnPositions.Length; p++)
            {
                do
                {
                    pos = Random.insideUnitCircle * spawnRangeDist;
                } while (!MinimumDistanceCheck(pos));

                spawnPositions[p] = pos;
            }
        }

        void SpawnAllAtOnce()
        {
            for (int p = 0; p < spawnPositions.Length; p++)
            {
                SpawnBerry(spawnPositions[p]);
            }
        }

        void SpawnBerry(Vector2 spawnPoint)
        {
            var berry = foodFactory.Spawn(GetLocation() + spawnPoint).GetComponent<FoodBerry>();
            berry.SetParentBush(this);
            berry.transform.SetParent(myTransform);
        }

        IEnumerator Respawn()
        {
            if (isTimeVariable)
            {
                float varTime1 = Random.Range(-variableTimeAmount, variableTimeAmount);
                float varTime2 = Random.Range(-variableTimeAmount, variableTimeAmount);
                timer += (varTime1 + varTime2)/2;
            }

            while (Application.isPlaying)
            {
                for (int p = 0; p < spawnPositions.Length; p++)
                {
                    yield return new WaitForSeconds(timer);

                    if (!Physics2D.OverlapPoint(GetLocation() + spawnPositions[p], MaskLayers.food))
                    {
                        SpawnBerry(spawnPositions[p]);
                    }
                }
            }
        }

        bool MinimumDistanceCheck(Vector2 point)
        {
            if (spawnPositions.Length == 0)
            {
                return true;
            }
            for (int p = 0; p < spawnPositions.Length; p++)
            {
                if (Vector2.Distance(point, spawnPositions[p]) < minimumDistance)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanBeTargeted(int team)
        {
            return true;
        }

        public Vector2 GetLocation()
        {
            return myTransform.position;
        }
    }
}