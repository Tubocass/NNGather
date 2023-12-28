using System.Collections.Generic;
using UnityEngine;

public class PrefabFactory : MonoBehaviour
{
    public GameObject prefab;
    public List<GameObject> pool = new List<GameObject>();
    int instanceId;

    GameObject Create(Vector2 location)
    {
        GameObject go = GameObject.Instantiate(prefab, location, Quaternion.identity);
        go.name = prefab.name + instanceId++;
        pool.Add(go);
        return go;
    }

    public GameObject Spawn(Vector3 location)
    {
        GameObject go;
        if (pool.Count > 0)
        {
            go = pool.Find(f => !f.activeSelf);
            if (go)
            {
                go.transform.position = location;
                go.SetActive(true);
            } else
            {
               go = Create(location);
            }
            return go;
        } else
        {
            go = Create(location);
            return go;
        }
    }
}