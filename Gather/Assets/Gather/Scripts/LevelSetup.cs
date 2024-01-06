using UnityEngine;

namespace gather
{
    public class LevelSetup : MonoBehaviour
    {
        Vector2[] startLocations;
        int startIndex;
        [SerializeField] GameObject locationPrefab;
        [SerializeField] int numSpawnPoints = 8;
        [SerializeField] float range = 500;
        [SerializeField] float startLocationMinimumDist;

        private void Awake()
        {
            //var children = transform.GetComponentsInChildren<Transform>();
            //startLocations = new Vector2[children.Length - 1];
            //for (int i = 0; i < startLocations.Length; i++) 
            //{
            //    startLocations[i] = children[i + 1].position;
            //}
        }

        public Vector2 GetStartLocation()
        {
            return startLocations[startIndex++ % startLocations.Length];
        }

        public void Generate()
        {
            startLocations = new Vector2[numSpawnPoints];
            Vector2 pos = Vector2.zero;
            for (int i = 0; i < numSpawnPoints; i++)
            {
                do
                {
                    float x = Random.Range(-range, range);
                    float y = Random.Range(-range, range);
                    pos = new Vector2(x, y);
                } while (FailsMinimumDistanceCheck(pos, startLocations, startLocationMinimumDist, i));

                startLocations[i] = pos;
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

    }
}