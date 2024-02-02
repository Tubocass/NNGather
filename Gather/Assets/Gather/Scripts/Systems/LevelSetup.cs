using UnityEngine;

namespace gather
{
    public class LevelSetup : MonoBehaviour
    {
        [SerializeField] GameObject FoodBushPrefab;
        [SerializeField] int numFoodBushes = 20;
        [SerializeField] float foodBushMinimumDist;
        [SerializeField] Transform foodParent;
        [Space]
        [SerializeField] int numSpawnPoints = 8;
        [SerializeField] float spawnPointMinimumDist;
        Vector2[] startLocations;
        int startIndex;
        float xRange, yRange;

        private void Awake()
        {
            Collider2D bgCollider = GetComponent<Collider2D>();
            xRange = bgCollider.bounds.extents.x;
            yRange = bgCollider.bounds.extents.y;
        }

        public Vector2 GetStartLocation()
        {
            return startLocations[startIndex++ % startLocations.Length];
        }

        public void Generate()
        {
            GenerateSpawnPoints();
            GenerateFood();
        }

        void GenerateSpawnPoints()
        {
            startLocations = new Vector2[numSpawnPoints];
            Vector2 pos = Vector2.zero;
            for (int i = 0; i < numSpawnPoints; i++)
            {
                do
                {
                    pos = RandomPoint();
                } while (FailsMinimumDistanceCheck(pos, startLocations, spawnPointMinimumDist, i));

                startLocations[i] = pos;
            }
        }

        void GenerateFood()
        {
            Vector2[] foodLocations = new Vector2[numFoodBushes];
            Vector2 pos = Vector2.zero;
            for (int i = 0; i < numFoodBushes; i++)
            {
                do
                {
                    pos = RandomPoint();
                } while (FailsMinimumDistanceCheck(pos, foodLocations, foodBushMinimumDist, i));

                foodLocations[i] = pos;
            }

            for (int i = 0; i < numFoodBushes; i++)
            {
                GameObject.Instantiate(FoodBushPrefab, foodLocations[i], Quaternion.identity, foodParent);
            }
        }

        bool FailsMinimumDistanceCheck(Vector2 point, Vector2[] positions, float distance, int previousAmount)
        {
            if (previousAmount == 0)
            {
                return false;
            }
            for (int p = 0; p < previousAmount; p++)
            {
                if (Vector2.Distance(point, positions[p]) < distance)
                {
                    return true;
                }
            }
            return false;
        }

        Vector2 RandomPoint()
        {
            return new Vector2(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));
        }

    }
}