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
			spawnPoints[i] = foodPile[i].transform.position;
			//foodPile[i].SetActive(false);
		}
		StartCoroutine(SpawnFood());
	}
	GameObject InitialSpawn(Vector3 position)
	{
		GameObject food = Instantiate(foodObj, position + new Vector3(0,.5f,0), Quaternion.identity) as GameObject;
		food.GetComponent<FoodObject>().SetLine(0, food.transform.position);
		food.GetComponent<FoodObject>().SetLine(1, this.transform.position);
		//food.SetActive(false);
		return food;
	}

	IEnumerator SpawnFood()
	{
		while(true)
		{
			for(int i = 0; i<foodPile.Length; i++)
			{
				if(!foodPile[i].gameObject.activeSelf)
				{
					foodPile[i].GetComponent<FoodObject>().Reset(spawnPoints[i]);
					yield return new WaitForSeconds(3f);
				}
			}
			yield return new WaitForSeconds(3f);
		} 
	}
}


