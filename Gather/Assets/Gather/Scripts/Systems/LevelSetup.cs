using UnityEngine;

namespace gather
{
    public class LevelSetup : MonoBehaviour
    {
        [SerializeField] GameObject FoodBushPrefab;
        [SerializeField] int numFoodBushes = 20;
        [SerializeField] float foodBushMinDist;
        [SerializeField] Transform foodParent;
        [Space(5)]
        [SerializeField] int numStartLocations = 8;
        [SerializeField] float startLocationMinDist;
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
            startLocations =  GenerateSpawnPoints(numStartLocations, startLocationMinDist);
            GenerateFood();
        }

        void GenerateFood()
        {
            Vector2[] foodLocations = GenerateSpawnPoints(numFoodBushes, foodBushMinDist);

            for (int i = 0; i < numFoodBushes; i++)
            {
                GameObject.Instantiate(FoodBushPrefab, foodLocations[i], Quaternion.identity, foodParent);
            }
        }

        Vector2[] GenerateSpawnPoints(int size, float minDist)
        {
            Vector2[] sapwnPoints = new Vector2[size];
            Vector2 pos;
            for (int i = 0; i < size; i++)
            {
                do
                {
                    pos = new Vector2(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));
                } while (FailsMinimumDistanceCheck(pos, sapwnPoints, minDist));

                sapwnPoints[i] = pos;
            }
            return sapwnPoints;
        }

        bool FailsMinimumDistanceCheck(Vector2 point, Vector2[] positions, float distance)
        {
            if (positions.Length == 0)
            {
                return false;
            }
            for (int p = 0; p < positions.Length; p++)
            {
                if (Vector2.Distance(point, positions[p]) <= distance)
                {
                    return true;
                }
            }
            return false;
        }
    }
}