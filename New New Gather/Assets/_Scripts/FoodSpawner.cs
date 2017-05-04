using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FoodSpawner : NetworkBehaviour 
{
	public Vector3 Location{get{return transform.position;}}
	[SerializeField] int amount = 4;
	[SerializeField] float radius = 3, clusterDist = 1;
	[SerializeField] GameObject foodObj;
 	GameObject[] foodPile;
	Vector3[] spawnPoints;
	bool bSpawnTime;
	string myTag;

	void OnEnable()
	{
		myTag = gameObject.tag;
		UnityEventManager.StartListeningBool("DayTime", DaySwitch);
		DaySwitch(GameController.instance.IsDayLight());
	}
	void OnDisable()
	{
		UnityEventManager.StopListeningBool("DayTime", DaySwitch);
	}

	void DaySwitch(bool b)
	{
		if(myTag.Equals("DayPlant"))
		bSpawnTime = b;
		else bSpawnTime = !b;
	}


	void Start () 
	{
		if(!isServer)
		return;

		foodPile = new GameObject[amount];
		spawnPoints = new Vector3[amount];
		GenerateLevel.SpawnObjects(amount, radius, clusterDist, Location, foodPile, InitialSpawn);//Spawn 
		for(int i = 0; i<amount; i++)//and then store the spawnpoint
		{
			spawnPoints[i] = foodPile[i].transform.position;
			foodPile[i].SetActive(false);
			foodPile[i].GetComponent<LineRenderer>().SetPosition(0,foodPile[i].transform.position);
			foodPile[i].GetComponent<LineRenderer>().SetPosition(1,transform.position);
		}
		StartCoroutine(SpawnFood());
//		for(int i = 0; i<amount; i++)
//		{
//			GameObject newFood = Instantiate(foodObj, spawnPoints[i], Quaternion.identity) as GameObject;
//			foodPile[i] = newFood.GetComponent<FoodObject>();
//			foodPile[i].gameObject.SetActive(false);
//		}
	}
	GameObject InitialSpawn(Vector3 position)
	{
		GameObject food = Instantiate(foodObj, position + new Vector3(0,.5f,0), Quaternion.identity) as GameObject;
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
					foodPile[i].GetComponent<FoodObject>().RpcReset(spawnPoints[i]);
//					foodPile[i].transform.position = spawnPoints[i];
//					foodPile[i].gameObject.SetActive(true);
					if(bSpawnTime)
					{
						yield return new WaitForSeconds(3f);
					}else yield return new WaitForSeconds(12f);
				}
			}
			yield return new WaitForSeconds(3f);
		} 
	}
}
