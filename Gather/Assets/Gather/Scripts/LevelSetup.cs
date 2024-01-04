using UnityEngine;

namespace gather
{
    public class LevelSetup : MonoBehaviour
    {
        Vector2[] startLocations;
        int start = 0;

        private void Awake()
        {
            var children = transform.GetComponentsInChildren<Transform>();
            startLocations = new Vector2[children.Length - 1];
            for (int i = 0; i < startLocations.Length; i++) 
            {
                startLocations[i] = children[i + 1].position;
            }
        }

        public Vector2 GetStartLocation()
        {
            return startLocations[start++ % startLocations.Length];
        }

    }
}