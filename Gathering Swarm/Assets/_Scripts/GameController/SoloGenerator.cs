using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoloGenerator : MonoBehaviour 
{
	public static float xx, zz;
	public int pits = 3, spClusterDist = 20, nightPlants = 5, nightPlantRadius = 15, nightPlantClusterDist = 5;
	[Space(10)]

	public int bots = 2, momsDistance = 20;
	[Space(10)]

	Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
	public GameObject Ground, NightPlantFab, DayPlantFab, ScarFab, SarlacFab, Sarlac_PitFab, EnemyMoMFab, MainMoMFab, NetStartFab, GlowFab;//prefabs
	[Space(5)]

	public static GameObject[] Pits;
	public GameObject SarlacDude;
	PlayerMomController[] playerMoMs;
	GameObject[] spawnPoints;
	Vector3 groundSize;
	int MoMCount, botCount, spCount, moms;
	public delegate GameObject SpawnFunction(Vector3 v);

	public void LoadLevelSettings(int numBots, int sarlacPits, int plantsPerPit)
	{
		pits = sarlacPits;
		nightPlants = plantsPerPit;
		bots = numBots;
	}
	public void PassInPlayers(PlayerMomController[] Players)
	{
		playerMoMs = Players;
	}
	public void Init() 
	{
		var ground = (GameObject)Instantiate(Ground, Vector3.zero, Quaternion.identity);
		ground.SetActive(true);
		//NetworkServer.Spawn(ground);
		groundSize = ground.GetComponent<MeshRenderer>().bounds.extents;
		xx = groundSize.x - groundSize.x/8;
		zz = groundSize.z - groundSize.z/8;

		moms = GameController.instance.numPlayers;
		Unit_Base.TotalCreated = 0;
		Pits = new GameObject[pits];
		//MoMs = new GameObject[moms];
		spawnPoints = new GameObject[moms];
	}
	public void Generate() 
	{
		//NetworkStartPoints
		SpawnObjects(moms, xx, momsDistance, Vector3.zero, spawnPoints, SpawnStartPositions);

		for(int i = 0; i<playerMoMs.Length; i++)
		{
			if(playerMoMs[i] != null)
			{
			  	//GameObject go = Instantiate(MoMs[i])as GameObject;
				SetMoMObj(playerMoMs[i]);
			}
		}
		for(int i = 0; i<bots; i++)
		{
			SpawnBots();
		}
		SarlacDude = SpawnSarlac();//The Sarlac needs to have its enable and disable functions rewritten
		SarlacDude.SetActive(false);
	
		//Sarlac Pits
		SpawnObjects(pits, xx, spClusterDist, Vector3.zero, Pits, SpawnSarlacPit);

		//Day Plants
		//SpawnObjects(dayScars, xx, dayPlantPitDistance, Vector3.zero, Pits, SpawnDayPlants); //need to make them separate from Sarlac pits
	}

	GameObject SpawnStartPositions(Vector3 position)
	{
		Vector3 height = new Vector3(0,0.5f,0);
		GameObject start = Instantiate(NetStartFab, position+height, Quaternion.identity) as GameObject;
		spCount++;
		return start;
	}
	GameObject SpawnMoM(Vector3 position)
	{
		GameObject newMoM;
		if(MoMCount==0)
		{
			newMoM = Instantiate(MainMoMFab, position, Quaternion.identity)as GameObject;
		}else {
			newMoM = Instantiate(EnemyMoMFab, position, Quaternion.identity)as GameObject;
		}
		newMoM.GetComponent<MoMController>().teamID = MoMCount;
		//Unit_Base.TeamSize[MoMCount] += 1;
		newMoM.GetComponent<MoMController>().TeamColor = Colors[MoMCount];
		newMoM.GetComponentInChildren<MeshRenderer>().material.color = Colors[MoMCount];
		MoMCount+=1;
		return newMoM;
	}
	GameObject SpawnBots()
	{
		GameObject newMoM;
		MoMController mom;
		Vector3 height = new Vector3(0,0.5f,0);
		newMoM = Instantiate(EnemyMoMFab, height, Quaternion.identity)as GameObject;
		mom = newMoM.GetComponent<MoMController>();
		mom.TeamColor = Colors[MoMCount];
		mom.teamID = MoMCount;
		SetMoMObj(mom);
		//NetworkServer.Spawn(newMoM);
		return newMoM;
	}
	public void SetMoMObj(MoMController newMoM)
	{
		GameController.instance.TeamSize[newMoM.teamID] += 1;
		//newMoM.GetComponentInChildren<MeshRenderer>().material.color = Colors[MoMCount];
		newMoM.transform.position = spawnPoints[MoMCount].transform.position;
		MoMCount+=1;
	}
	GameObject SpawnSarlac()
	{
		GameObject sarlac = Instantiate(SarlacFab, Vector3.zero,Quaternion.identity) as GameObject;
		return sarlac;
	}
	GameObject SpawnSarlacPit(Vector3 position)
	{
		Vector3 height = new Vector3(0,0.5f,0);
		GameObject newPit =	Instantiate(Sarlac_PitFab, position, Quaternion.identity)as GameObject;
		int g = UnityEngine.Random.Range(0,3);// number of glow rocks
		nightPlants = UnityEngine.Random.Range(3,7);// number of plants
		GameObject[] plantObjs = new GameObject[nightPlants];
		SpawnObjects(nightPlants, nightPlantRadius,nightPlantClusterDist, position+height, plantObjs, (Vector3 pos)=>
		{//SpawnNightPlants() essentially
			GameObject obj = Instantiate(NightPlantFab, pos, Quaternion.identity)as GameObject;
			Vector3 dir = pos - position;
			dir = dir/2+ new Vector3(Random.Range(-4f,4f), 0, 0);
			NetworkTestSpawner fs = obj.GetComponent<NetworkTestSpawner>();
//			fs.AddLineSegment(pos);
//			fs.AddLineSegment(pos-dir);
//			fs.AddLineSegment(position);

//			obj.GetComponent<LineRenderer>().SetPosition(0, pos);
//			obj.GetComponent<LineRenderer>().SetPosition(1, (pos - dir));
//			obj.GetComponent<LineRenderer>().SetPosition(2, position);
			return obj; 
		});
		//SpawnObjects(NightPlantFab, nightPlants, nightPlantRadius, nightPlantClusterDist, position);
//		for(int i = 0; i<nightPlants; i++)
//		{
//			Vector3 plantPos = plantObjs[i].transform.position;
//			Vector3 dir = plantPos - position;
//			dir = dir/2+ new Vector3(Random.Range(-4f,4f), 0, 0);
//			plantObjs[i].GetComponent<LineRenderer>().SetPosition(0, plantPos);
//			plantObjs[i].GetComponent<LineRenderer>().SetPosition(1, (plantPos- dir));
//			plantObjs[i].GetComponent<LineRenderer>().SetPosition(2, position);
//		}
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
	//[Server]
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
				//NetworkServer.Spawn(objs[created]);
				created++;
			}else
			{
				nearestLoc = NearestTarget(objs, spawnPoint);
				if((nearestLoc-spawnPoint).sqrMagnitude>clusterDistSqrd)
				{
					objs[created] = create(spawnPoint);
					//NetworkServer.Spawn(objs[created]);
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
							//NetworkServer.Spawn(objs[created]);
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
