using UnityEngine;

namespace Gather
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