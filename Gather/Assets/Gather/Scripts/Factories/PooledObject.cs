using UnityEngine;

namespace gather
{
    public class PooledObject : MonoBehaviour
    {
        public PrefabFactory factoryPool;

        public void ReleaseToPool()
        {
            factoryPool.ReleaseToPool(this);
        }
    }
}