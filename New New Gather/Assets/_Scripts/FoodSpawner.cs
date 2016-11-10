using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour 
{
	public Vector3 Location{get{return transform.position;}}
	[SerializeField] int amount;
	[SerializeField] GameObject foodObj;
	[SerializeField] FoodObject[] foodPile;
	[SerializeField] Vector3[] spawnPoints;
	bool bSpawnTime;
	string myTag;

	void OnEnable()
	{
		myTag = gameObject.tag;
		UnityEventManager.StartListening("DayTime", DaySwitch);

	}
	void OnDisable()
	{
		UnityEventManager.StopListening("DayTime", DaySwitch);
	}
	void DaySwitch(bool b)
	{
		if(myTag.Equals("DayPlant"))
		bSpawnTime = b;
		else bSpawnTime = !b;

		if(bSpawnTime)
		{
			StartCoroutine(SpawnFood());
		}
	}

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
		while(bSpawnTime)
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
			yield return new WaitForSeconds(3f);
		}
	}
}
