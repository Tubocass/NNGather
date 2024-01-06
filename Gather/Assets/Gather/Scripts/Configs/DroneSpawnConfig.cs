using System.Collections;
using UnityEngine;

namespace gather
{
    [CreateAssetMenu]
    public class DroneSpawnConfig : ScriptableObject
    {
        public int farmerCost;
        public int fighterCost;
        public int farmerCap;
        public int fighterCap;
    }
}