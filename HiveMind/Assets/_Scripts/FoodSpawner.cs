using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class FoodSpawner : NetworkBehaviour 
{
	public Vector3 Location{get{return transform.position;}}
	[SerializeField] int amount = 4;
	[SerializeField] float radius = 3, clusterDist = 1;
	[SerializeField] GameObject foodObj;
 	GameObject[] foodPile;
	Vector3[] spawnPoints;
	public SyncListVector3 lines = new SyncListVector3();
	LineRenderer vine;
	int segments = 0;
	bool bSpawnTime;
	string myTag;

	void OnEnable()
	{
		myTag = gameObject.tag;
		vine = GetComponent<LineRenderer>();
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

	public void AddLineSegment(Vector3 point)
	{
		lines.Add(point);
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		//vine.SetPositions();
		if(isServer)
		{
			vine.positionCount =lines.Count;
			for(int i = 0; i<lines.Count;i++)
			{
				vine.SetPosition(i,lines[i]);
			}
		}
	}
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
			foodPile[i].GetComponent<FoodObject>().SetLine(0,foodPile[i].transform.position);
			foodPile[i].GetComponent<FoodObject>().SetLine(1,transform.position);
			foodPile[i].GetComponent<FoodObject>().Destroy();
		}
		StartCoroutine(SpawnFood());
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
