using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PrefabContainer : ScriptableObject
{
    public Hashtable prefabList;
    public GameObject[] objectPrefabs, tilePrefabs;
}
