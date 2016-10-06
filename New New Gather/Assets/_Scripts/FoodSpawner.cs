using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour 
{
	[SerializeField] int amount;
	[SerializeField] GameObject foodObj;
	[SerializeField] FoodObject[] foodPile;
	[SerializeField] Vector3[] spawnPoints;
	public Vector3 Location{get{return transform.position;}}

	void Start () 
	{
		foodPile = new FoodObject[amount];
		spawnPoints = new Vector3[amount];
		for(int i = 0; i<amount; i++)
		{
			spawnPoints[i] = transform.position + new Vector3(Random.Range(-3,3), 0, Random.Range(-3,3));
		}
		for(int i = 0; i<amount; i++)
		{
			GameObject newFood = Instantiate(foodObj, spawnPoints[i], Quaternion.identity) as GameObject;
			foodPile[i] = newFood.GetComponent<FoodObject>();
		}
		StartCoroutine(SpawnFood());
	}
	
	IEnumerator SpawnFood()
	{
		while(true)
		{
			for(int i = 0; i<foodPile.Length; i++)
			{
				if(!foodPile[i].gameObject.activeSelf)
				{
					foodPile[i].transform.position = spawnPoints[i];
					foodPile[i].gameObject.SetActive(true);
					yield return new WaitForSeconds(3f);
				}
			}
			yield return new WaitForSeconds(5f);
		}
	}
}
