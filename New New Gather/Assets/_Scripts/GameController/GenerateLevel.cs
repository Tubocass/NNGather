﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class GenerateLevel : NetworkBehaviour 
{
	public static float xx, zz;
	public int pits = 3, spClusterDist = 20, nightPlants = 5, nightPlantRadius = 15, nightPlantClusterDist = 5;
	[Space(10)]

	public int dayScars = 5, dayPlantPitDistance = 30;
	[Space(10)]

	public int moms = 3, momsDistance = 20;
	[Space(10)]

	[SerializeField] Color[] Colors;
	public GameObject Ground, NightPlantFab, DayPlantFab, ScarFab, Sarlac_PitFab, EnemyMoMFab, MainMoMFab, GlowFab;//prefabs
	[Space(5)]

	public static GameObject[] Pits;
	GameObject[] MoMs;
	Vector3 groundSize;
	int MoMCount;
	public delegate GameObject SpawnFunction(Vector3 v);

	public void Generate() 
	{
		Unit_Base.TeamSize = new int[10];
		Unit_Base.TotalCreated = 0;
		var ground = (GameObject)Instantiate(Ground, Vector3.zero, Quaternion.identity);
		NetworkServer.Spawn(ground);
		ground.SetActive(true);
		groundSize = ground.GetComponent<MeshRenderer>().bounds.extents;
		xx = groundSize.x - groundSize.x/8;
		zz = groundSize.z - groundSize.z/8;
		Pits = new GameObject[pits];
		MoMs = new GameObject[moms];

		//Sarlac Pits
		SpawnObjects(pits, xx, spClusterDist, Vector3.zero, Pits, SpawnSarlacPit);

		//Day Plants
		//SpawnObjects(dayScars, xx, dayPlantPitDistance, Vector3.zero, Pits, SpawnDayPlants); //need to make them separate from Sarlac pits

		//MoMs
		SpawnObjects(moms, xx, momsDistance, Vector3.zero, MoMs, SpawnMoM);
	}

	GameObject SpawnMoM(Vector3 position)
	{
		GameObject newMoM;
		Vector3 height = new Vector3(0,0.5f,0);
		if(MoMCount==0)
		{
			newMoM = Instantiate(MainMoMFab, position+height, Quaternion.identity)as GameObject;
			//NetworkServer.SpawnWithClientAuthority(newMoM, PlayerMomController.MainMoM.gameObject);
		}else {
			newMoM = Instantiate(EnemyMoMFab, position+height, Quaternion.identity)as GameObject;
		}
		newMoM.GetComponent<MoMController>().teamID = MoMCount;
		Unit_Base.TeamSize[MoMCount] += 1;
		newMoM.GetComponent<MoMController>().TeamColor = Colors[MoMCount];
		newMoM.GetComponentInChildren<MeshRenderer>().material.color = Colors[MoMCount];
		MoMCount+=1;
		return newMoM;
	}
	GameObject SpawnSarlacPit(Vector3 position)
	{
		GameObject newPit =	Instantiate(Sarlac_PitFab, position, Quaternion.identity)as GameObject;
		int g = UnityEngine.Random.Range(0,3);// number of glow rocks
		nightPlants = UnityEngine.Random.Range(3,7);// number of plants
		GameObject[] plantObjs = new GameObject[nightPlants];
		SpawnObjects(nightPlants, nightPlantRadius,nightPlantClusterDist, position, plantObjs, (Vector3 pos)=>
		{
			GameObject obj = Instantiate(NightPlantFab, pos, Quaternion.identity)as GameObject;
			return obj; 
		});
		//SpawnObjects(NightPlantFab, nightPlants, nightPlantRadius, nightPlantClusterDist, position);
		for(int i = 0; i<nightPlants; i++)
		{
			Vector3 plantPos = plantObjs[i].transform.position;
			Vector3 dir = plantPos - position;
			dir = dir/2+ new Vector3(Random.Range(-4f,4f), 0, 0);
			plantObjs[i].GetComponent<LineRenderer>().SetPosition(0, plantPos);
			plantObjs[i].GetComponent<LineRenderer>().SetPosition(1, (plantPos- dir));
			plantObjs[i].GetComponent<LineRenderer>().SetPosition(2, position);
		}
		if(g>0)
		SpawnObjects(GlowFab, g, 15, nightPlantClusterDist, position);
		return newPit;
	}

	GameObject SpawnDayPlants(Vector3 position)
	{
		//float angle = Vector3.Angle(position, new Vector3());
		Quaternion rand =  Quaternion.AngleAxis(UnityEngine.Random.Range(0,360), Vector3.up);//UnityEngine.Random.Range(0,360)
		GameObject scar = Instantiate(ScarFab, position, rand)as GameObject;
	
		//plants = UnityEngine.Random.Range(3,7);// number of plants
		SpawnObjects(DayPlantFab, 2, nightPlantRadius, nightPlantClusterDist, position);

		return scar;
	}

	public static void SpawnObjects(GameObject fab, int amount, float radius, float clusterDist, Vector3 position)
	{
		GameObject[] objs = new GameObject[amount]; 
		SpawnObjects(amount, radius, clusterDist, position, objs, (Vector3 pos)=>
		{
			GameObject obj = Instantiate(fab, pos, Quaternion.identity)as GameObject;
			return obj; 
		});
	}

	public static void SpawnObjects(int amount, float radius, float clusterDist, Vector3 position, GameObject[] objs, SpawnFunction create)//, LayerMask mask
	{
		//GameObject[] objs = new GameObject[amount]; 
		float clusterDistSqrd = clusterDist*clusterDist;
		int created = 0;

		if(amount<=0)
			return;

		do{
			Vector3 spawnPoint = new Vector3(UnityEngine.Random.Range(-radius,radius)+position.x, 0f, UnityEngine.Random.Range(-radius,radius)+position.z);
			Mathf.Clamp(spawnPoint.x, -GenerateLevel.xx, GenerateLevel.xx);
			Mathf.Clamp(spawnPoint.z, -GenerateLevel.zz, GenerateLevel.zz);
			Vector3 nearestLoc;

			if(objs[0] == null)
			{
				objs[created] = create(spawnPoint);
				created++;
			}else
			{
				nearestLoc = NearestTarget(objs, spawnPoint);
				if((nearestLoc-spawnPoint).sqrMagnitude>clusterDistSqrd)
				{
					objs[created] = create(spawnPoint);
					created++;
				}else
				{
					do{
						spawnPoint = new Vector3(UnityEngine.Random.Range(-radius,radius)+position.x, 0f, UnityEngine.Random.Range(-radius,radius)+position.z);
						Mathf.Clamp(spawnPoint.x, -GenerateLevel.xx, GenerateLevel.xx);
						Mathf.Clamp(spawnPoint.z, -GenerateLevel.zz, GenerateLevel.zz);
						nearestLoc = NearestTarget(objs, spawnPoint);
						if((nearestLoc-spawnPoint).sqrMagnitude>clusterDistSqrd)
						{
							objs[created] = create(spawnPoint);
							created++;
						}
					}while((nearestLoc-spawnPoint).sqrMagnitude<clusterDistSqrd);
				}
			}
		}while(created<amount);
	}


	public static Vector3 NearestTarget(GameObject[] Objects, Vector3 targetLoc)
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
