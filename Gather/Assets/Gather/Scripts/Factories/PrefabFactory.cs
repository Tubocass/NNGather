using System.Collections.Generic;
using UnityEngine;
using gather;

public abstract class PrefabFactory : MonoBehaviour
{
    public PooledObject prefab;
    public Stack<PooledObject> pool = new Stack<PooledObject>();
    int instanceId;

    PooledObject Create(Vector2 location)
    {
        PooledObject po = PooledObject.Instantiate(prefab, location, Quaternion.identity);
        po.name = prefab.name + "_" + instanceId++;
        po.factoryPool = this;
        return po;
    }

    public PooledObject Spawn(Vector3 location)
    {
        PooledObject po;
        if (pool.Count > 0)
        {
            po = pool.Pop();
            po.gameObject.transform.position = location;
            po.gameObject.SetActive(true);
            return po;
        } else
        {
            po = Create(location);
            return po;
        }
    }

    public void ReleaseToPool (PooledObject po) 
    {
        //po.gameObject.SetActive(false);
        pool.Push(po);
    }
}