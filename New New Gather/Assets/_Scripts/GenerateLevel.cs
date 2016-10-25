using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateLevel : MonoBehaviour 
{
	public GameObject Ground, Plant, Sarlac_Pit, EnemyMoMFab, MainMoMFab;
	public Vector3 groundSize;
	public int plants = 5, pits = 3, plantClusterDist = 5, spClusterDist = 20, moms = 2, momsDistance = 20;
	public Color[] Colors;
	GameObject[] Pits;
	GameObject[] MoMs;
	//List<FoodSpawner> PlantList = new List<FoodSpawner>();
	GameObject spawn;
	Vector3 spawnPoint;
	float plantClustSqrd, spClusterSqrd, mmDistanceSqrd;
	float xx, zz;

	void Start () 
	{
		plantClustSqrd = plantClusterDist*plantClusterDist; //causing problems with lvl generation
		spClusterSqrd = spClusterDist*spClusterDist;
		mmDistanceSqrd = momsDistance * momsDistance;
		groundSize = Ground.GetComponent<MeshRenderer>().bounds.extents;
		xx = groundSize.x - groundSize.x/8;
		zz = groundSize.z- groundSize.z/8;
		Pits = new GameObject[pits];
		MoMs = new GameObject[moms];
		int sp = 0, mm = 0;
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
				if((nearestLoc-spawnPoint).sqrMagnitude>spClusterSqrd)
				{
					Pits[sp] = SpawnSarlacPit(spawnPoint);
					sp++;
				}else{

					spawnPoint = new Vector3(Random.Range(-xx,xx), 0.5f, Random.Range(-zz,zz));
					nearestLoc = NearestTarget(Pits, spawnPoint);
					if((nearestLoc-spawnPoint).sqrMagnitude>spClusterSqrd)
					{
						Pits[sp] = SpawnSarlacPit(spawnPoint);
						sp++;
					}
				}
			}
		}while(sp<pits);

		do{
			spawnPoint = new Vector3(Random.Range(-xx,xx), 0.5f, Random.Range(-zz,zz));

			if(mm==0)
			{
				MoMs[mm] = Instantiate(MainMoMFab, spawnPoint, Quaternion.identity)as GameObject;
				//UnityEventManager.TriggerEvent("MainMomChange",MoMs[mm]);
				//fd = spawn.GetComponent<FoodSpawner>();
				//PlantList.Add(fd);
				mm++;
			}else
			{
				Vector3 nearestLoc = NearestTarget(MoMs, spawnPoint);//Find(l=> (l.Location-spawnPoint).sqrMagnitude<clusterDist)
				if((nearestLoc-spawnPoint).sqrMagnitude>mmDistanceSqrd)
				{
					MoMs[mm] = SpawnMoM(spawnPoint, mm);
					mm++;
				}else{

					spawnPoint = new Vector3(Random.Range(-xx,xx), 0.5f, Random.Range(-zz,zz));
					nearestLoc = NearestTarget(MoMs, spawnPoint);
					if((nearestLoc-spawnPoint).sqrMagnitude>mmDistanceSqrd)
					{
						MoMs[mm] = SpawnMoM(spawnPoint, mm);
						mm++;
					}
				}
			}
		}while(mm<moms);

	}

	GameObject SpawnMoM(Vector3 position, int count)
	{
		GameObject newMoM =	Instantiate(EnemyMoMFab, position, Quaternion.identity)as GameObject;
		newMoM.GetComponent<MoMController>().TeamColor = Colors[count];
		newMoM.GetComponentInChildren<MeshRenderer>().material.color = Colors[count];
		return newMoM;
	}
	GameObject SpawnSarlacPit(Vector3 position)
	{
		GameObject newPit =	Instantiate(Sarlac_Pit, position, Quaternion.identity)as GameObject;
		PitController.Pits.Add(newPit.GetComponent<PitController>());

		int pl = 0;
		//float minX = -groundSize.x, maxX = groundSize.x, minZ = groundSize.z, maxZ = groundSize.z;;
		GameObject[] flowers = new GameObject[plants]; 
		do{
			spawnPoint = new Vector3(Random.Range(-plantClustSqrd,plantClustSqrd)+position.x, 0.5f, Random.Range(-plantClustSqrd,plantClustSqrd)+position.z);
			Mathf.Clamp(spawnPoint.x, -xx, xx);
			Mathf.Clamp(spawnPoint.z, -zz, zz);
			Vector3 nearestLoc;

			if(pl<1)
			{
				flowers[pl] = Instantiate(Plant, spawnPoint, Quaternion.identity)as GameObject;
				pl++;
			}else
			{
				nearestLoc = NearestTarget(flowers, spawnPoint);//Find(l=> (l.Location-spawnPoint).sqrMagnitude<clusterDist)
				if((nearestLoc-spawnPoint).sqrMagnitude>plantClusterDist)
				{
					flowers[pl] = Instantiate(Plant, spawnPoint, Quaternion.identity)as GameObject;
					pl++;
				}else{

					spawnPoint = new Vector3(Random.Range(-plantClustSqrd,plantClustSqrd)+position.x, 0.5f, Random.Range(-plantClustSqrd,plantClustSqrd)+position.z);
					Mathf.Clamp(spawnPoint.x, -xx, xx);
					Mathf.Clamp(spawnPoint.z, -zz, zz);
					nearestLoc = NearestTarget(flowers, spawnPoint);
					if((nearestLoc-spawnPoint).sqrMagnitude>plantClusterDist)
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
