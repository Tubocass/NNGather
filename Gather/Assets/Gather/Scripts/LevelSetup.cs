using UnityEngine;

namespace gather
{
    public class LevelSetup : MonoBehaviour
    {
        [SerializeField] Vector2[] startLocations;
        int start = 0;

        public Vector2 GetStartLocation()
        {
            return startLocations[start++ % startLocations.Length];
        }

    }
}