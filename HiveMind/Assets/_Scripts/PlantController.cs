using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantController : MonoBehaviour 
{
	public Vector3 Location{get{return transform.position;}}
	//[SerializeField] int amount = 4;
	//[SerializeField] float radius = 3, clusterDist = 1;
	[SerializeField] GameObject foodObj;
 	GameObject[] foodPile;
	Vector3[] spawnPoints;
	//public SyncListVector3 lines = new SyncListVector3();
	LineRenderer[] vines;
	bool bSpawnTime;
	//string myTag;
		
	void Start () 
	{
		ObjectPool.CreatePool("Foods",420,foodObj);
	}
		
	void SpawnFood()
	{
	}
}
