using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateLevel : MonoBehaviour 
{
	public GameObject Ground, Plant, Sarlac_Pit;
	public Vector3 groundSize;
	public int plants = 5, pits = 3, clusterDist = 5;
	GameObject[] Pits;
	//List<FoodSpawner> PlantList = new List<FoodSpawner>();
	GameObject spawn;
	Vector3 spawnPoint;

	void Start () 
	{
		clusterDist *= clusterDist; 
		groundSize = Ground.GetComponent<MeshRenderer>().bounds.extents;
		float xx = groundSize.x - groundSize.x/8, zz = groundSize.z- groundSize.z/8;
		Pits = new GameObject[pits];
		int sp = 0;
		do{
			spawnPoint = new Vector3(Random.Range(-xx,xx), 0.5f, Random.Range(-zz,zz));

			if(sp==0)
			{
				Pits[sp] = SpawnSarlacPit(spawnPoint);
				//fd = spawn.GetComponent<FoodSpawner>();
				//PlantList.Add(fd);
				sp++;
			}else
			{
				Vector3 nearestLoc = NearestTarget(Pits, spawnPoint);//Find(l=> (l.Location-spawnPoint).sqrMagnitude<clusterDist)
				if((nearestLoc-spawnPoint).sqrMagnitude>clusterDist)
				{
					Pits[sp] = SpawnSarlacPit(spawnPoint);
					sp++;
				}else{

					spawnPoint = new Vector3(Random.Range(-xx,xx), 0.5f, Random.Range(-zz,zz));
					nearestLoc = NearestTarget(Pits, spawnPoint);
					if((nearestLoc-spawnPoint).sqrMagnitude>clusterDist)
					{
						Pits[sp] = SpawnSarlacPit(spawnPoint);
						sp++;
					}
				}
			}
		}while(sp<pits);

	}

	GameObject SpawnSarlacPit(Vector3 position)
	{
		GameObject newPit =	Instantiate(Sarlac_Pit, position, Quaternion.identity)as GameObject;

		int pl = 0;
		GameObject[] flowers = new GameObject[plants]; 
		do{
			spawnPoint = new Vector3(Random.Range(-clusterDist,clusterDist)+position.x, 0.5f, Random.Range(-clusterDist,clusterDist)+position.z);
			Mathf.Clamp(spawnPoint.x, -groundSize.x+8, groundSize.x-8);
			Mathf.Clamp(spawnPoint.z, -groundSize.z+8, groundSize.z-8);
			Vector3 nearestLoc;

			if(pl<1)
			{
				flowers[pl] = Instantiate(Plant, spawnPoint, Quaternion.identity)as GameObject;
				pl++;
			}else
			{
				nearestLoc = NearestTarget(flowers, spawnPoint);//Find(l=> (l.Location-spawnPoint).sqrMagnitude<clusterDist)
				if((nearestLoc-spawnPoint).sqrMagnitude>clusterDist)
				{
					flowers[pl] = Instantiate(Plant, spawnPoint, Quaternion.identity)as GameObject;
					pl++;
				}else{

					spawnPoint = new Vector3(Random.Range(-clusterDist,clusterDist), 0.5f, Random.Range(-clusterDist,clusterDist));
					Mathf.Clamp(spawnPoint.x, -groundSize.x+8, groundSize.x-8);
					Mathf.Clamp(spawnPoint.z, -groundSize.z+8, groundSize.z-8);
					nearestLoc = NearestTarget(flowers, spawnPoint);
					if((nearestLoc-spawnPoint).sqrMagnitude>clusterDist)
					{
						flowers[pl] = Instantiate(Plant, spawnPoint, Quaternion.identity)as GameObject;
						pl++;
					}
				}
			}
		}while(pl<plants);
		return newPit;
	}

//	FoodSpawner TargetNearest(List<FoodSpawner> ListOT, Vector3 targetLoc)
//	{
//		float nearestFoodDist, newDist;
//		FoodSpawner food = null;
//
//		//foods = Foods.FindAll(e=> e.CanBeTargetted && (e.Location-Location).sqrMagnitude<sqrDist);
//
//		if(ListOT.Count>0)
//		{
//			nearestFoodDist = (ListOT[0].Location-targetLoc).sqrMagnitude; //Vector3.Distance(Location,enemies[0].Location);
//			foreach(FoodSpawner f in ListOT)
//			{
//				newDist = (f.Location-targetLoc).sqrMagnitude;//Vector3.Distance(Location,unit.Location);
//				if(newDist <= nearestFoodDist)
//				{
//					nearestFoodDist = newDist;
//					food = f;
//				}
//			}
//		}
//		return food;
//	}
	Vector3 NearestTarget(GameObject[] Objects, Vector3 targetLoc)
	{
		float nearestDist, newDist;
		GameObject obj = null;

		//foods = Foods.FindAll(e=> e.CanBeTargetted && (e.Location-Location).sqrMagnitude<sqrDist);

		if(Objects.Length>0)
		{
			nearestDist = (Objects[0].transform.position-targetLoc).sqrMagnitude; //Vector3.Distance(Location,enemies[0].Location);
			foreach(GameObject o in Objects)
			{
				if(o!=null)
				{
					newDist = (o.transform.position-targetLoc).sqrMagnitude;//Vector3.Distance(Location,unit.Location);
					if(newDist <= nearestDist)
					{
						nearestDist = newDist;
						obj = o;
					}
				}
			}
		}
		return obj.transform.position;
	}
}
