using System.Collections;
using UnityEngine;

namespace gather
{
    [CreateAssetMenu]
    public class SpawnConfig : ScriptableObject
    {
        public int farmerCost;
        public int fighterCost;
        public int farmerCap;
        public int fighterCap;
    }
}