using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTestSpawner : NetworkBehaviour 
{
	public Vector3 Location{get{return transform.position;}}
	[SerializeField] int amount = 4;
	[SerializeField] float radius = 3, clusterDist = 1;
	[SerializeField] GameObject foodObj;
 	GameObject[] foodPile;
	Vector3[] spawnPoints;

	void Start () 
	{
		if(!isServer)
		return;

		foodPile = new GameObject[amount];
		spawnPoints = new Vector3[amount];
		GenerateLevel.SpawnObjects(amount, radius, clusterDist, Location, foodPile, InitialSpawn, LayerMask.NameToLayer("Food"));
		for(int i = 0; i<amount; i++)
		{
			//foodPile[i] = (GameObject)Instantiate(foodObj, transform.position+CreateSpawnPoint(3), Quaternion.identity);
			//NetworkServer.Spawn(foodPile[i]);
			spawnPoints[i] = foodPile[i].transform.position;
			foodPile[i].GetComponent<FoodObject>().SetLine(0,foodPile[i].transform.position);
			foodPile[i].GetComponent<FoodObject>().SetLine(1,transform.position);
			//foodPile[i].SetActive(false);
		}
		StartCoroutine(SpawnFood());
	}
	GameObject InitialSpawn(Vector3 position)
	{
		GameObject food = Instantiate(foodObj, position + new Vector3(0,.5f,0), Quaternion.identity) as GameObject;
		//food.SetActive(false);
		return food;
	}
	Vector3 CreateSpawnPoint(float radius)
	{
		Vector2 point = Random.insideUnitCircle;
		point = point*radius;
		return new Vector3(point.x, 0.5f , point.y);
	}

	IEnumerator SpawnFood()
	{
		while(true)
		{
			for(int i = 0; i<foodPile.Length; i++)
			{
				if(!foodPile[i].gameObject.activeSelf)
				{
					foodPile[i].GetComponent<FoodObject>().RpcReset(spawnPoints[i]);
					yield return new WaitForSeconds(3f);
				}
			}
			yield return new WaitForSeconds(3f);
		} 
	}
}


