﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.IO;

public class GenerateLevel : NetworkBehaviour 
{
	[SerializeField] static float xx=80, zz=80;
	[SerializeField] LayerMask envMask, unitMask;
	[SerializeField] int pits = 3, spClusterDist = 20, plantsPerPit = 5, nightPlantRadius = 15, nightPlantClusterDist = 5, momsDistance = 20;
	[Space(10)]
	public int bots = 2;
	[Space(10)]
	Color gatherColor = new Color(0.765f,0.225f,0.638f,1f);
	Color[] Colors;// = new Color[] { gatherColor, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
	bool[] availableColors;
	public GameObject Ground, NightPlantFab, DayPlantFab, ScarFab, SarlacFab, Sarlac_PitFab, EnemyMoMFab, MainMoMFab, NetStartFab, GlowFab;//prefabs
	[Space(5)]
	public static GameObject[] Pits;
	public GameObject SarlacDude;
	PlayerMomController[] playerMoMs;
	GameObject[] spawnPoints;
	Vector3 groundSize;
	Vector3 height = new Vector3(0,0.5f,0);
	[SyncVar]int MoMCount, botCount, spCount, moms;
	public delegate GameObject SpawnFunction(Vector3 v);
	float xd, zd;//for debugging
    private string gameDataProjectFilePath = "/StreamingAssets/data.json";
    LevelProperties levelProps;

    private void LoadGameData()
    {
        string filePath = Application.dataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            levelProps = JsonUtility.FromJson<LevelProperties>(dataAsJson);
        }
        else
        {
            levelProps = new LevelProperties();
        }
    }
    public void PassInPlayers(PlayerMomController[] Players)
	{
		playerMoMs = Players;
		for(int c = 0; c<playerMoMs.Length; c++)
		{
			for(int a = 0; a<availableColors.Length; a++)
			{
				if(playerMoMs[c].TeamColor.Equals(Colors[a]))
				{
					availableColors[a] = false;
					break;
				}
			}
		}
	}
	Color SelectColor()
	{
		for(int c = 0; c<availableColors.Length; c++)
		{
			if(availableColors[c])
			{
				availableColors[c] = false;
				return Colors[c];
			}
		}
		return Color.white;
	}
	public void Init() 
	{
        LoadGameData();
		var ground = (GameObject)Instantiate(Ground, Vector3.zero, Quaternion.identity);
		ground.SetActive(true);
		NetworkServer.Spawn(ground);
		groundSize = ground.GetComponent<MeshRenderer>().bounds.extents;
		xx = groundSize.x - groundSize.x/20;
		zz = groundSize.z - groundSize.z/20;
		xd=xx;
		zd=zz;
        Colors = new Color[] { gatherColor, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
        //Colors = levelProps.teamColors;
		availableColors = new bool[Colors.Length];
		for(int c = 0; c<availableColors.Length; c++)
		{
			availableColors[c] = true;
		}
	}
	public void Generate() 
	{
        bots = levelProps.bots;
		moms = GameController.Instance.numPlayers;
		Pits = new GameObject[pits];
		spawnPoints = new GameObject[moms];
		//NetworkStartPoints
		SpawnObjects(moms, xx, momsDistance, Vector3.zero, spawnPoints, SpawnStartPositions, unitMask);

		for(int i = 0; i<playerMoMs.Length; i++)
		{
			if(playerMoMs[i] != null)
			{
				SetMoMObj(playerMoMs[i]);
			}
		}
		for(int i = 0; i<bots; i++)
		{
			SpawnEnemyMoMs();
		}
		SarlacDude = SpawnSarlac();//The Sarlac needs to have its enable and disable functions rewritten
		SarlacDude.SetActive(false);
		//Sarlac Pits
		SpawnObjects(pits, xx, spClusterDist, Vector3.zero, Pits, SpawnSarlacPit, envMask);
	}

	GameObject SpawnStartPositions(Vector3 position)
	{
		GameObject start = Instantiate(NetStartFab, position+height, Quaternion.identity) as GameObject;
		spCount++;
		return start;
	}

	GameObject SpawnEnemyMoMs()
	{
		GameObject newMoM;
		MoMController mom;
		newMoM = Instantiate(EnemyMoMFab, height, Quaternion.identity)as GameObject;
		mom = newMoM.GetComponent<MoMController>();
		mom.TeamColor = SelectColor();
		mom.teamID = MoMCount;
		SetMoMObj(mom);
		NetworkServer.Spawn(newMoM);
		return newMoM;
	}
	public void SetMoMObj(MoMController newMoM)
	{
		GameController.Instance.TeamSize[newMoM.teamID] += 1;
		newMoM.transform.position = spawnPoints[MoMCount].transform.position;
		MoMCount+=1;
	}
	GameObject SpawnSarlac()
	{
		GameObject sarlac = Instantiate(SarlacFab, Vector3.zero,Quaternion.identity) as GameObject;
		NetworkServer.Spawn(sarlac);
		return sarlac;
	}
	GameObject SpawnSarlacPit(Vector3 position)
	{
		GameObject newPit =	Instantiate(Sarlac_PitFab, position, Quaternion.identity)as GameObject;
		int g = UnityEngine.Random.Range(0,3);// number of glow rocks
		GameObject[] plantObjs = new GameObject[plantsPerPit];
		SpawnObjects(plantsPerPit, nightPlantRadius,nightPlantClusterDist, position+height, plantObjs, (Vector3 pos)=>
		{//SpawnNightPlants() essentially
			GameObject obj = Instantiate(NightPlantFab, pos, Quaternion.identity)as GameObject;
			Vector3 dir = pos - position;
			dir = dir/2+ new Vector3(Random.Range(-4f,4f), 0, 0);

			obj.GetComponent<LineRenderer>().SetPosition(0, pos);
			obj.GetComponent<LineRenderer>().SetPosition(1, (pos - dir));
			obj.GetComponent<LineRenderer>().SetPosition(2, position);
			obj.transform.SetParent(newPit.transform);
			return obj; 
		},envMask);
		if(g>0)
		SpawnObjects(GlowFab, g, 15, nightPlantClusterDist, position, envMask);
		return newPit;
	}

	public static void SpawnObjects(GameObject fab, int amount, float radius, float clusterDist, Vector3 position, LayerMask mask)
	{
		GameObject[] objs = new GameObject[amount]; 
		SpawnObjects(amount, radius, clusterDist, position, objs, (Vector3 pos)=>
		{
			GameObject obj = Instantiate(fab, pos, Quaternion.identity)as GameObject;
			return obj; 
		},mask);
	}
	[Server]
	public static void SpawnObjects(int amount, float radius, float clusterDist, Vector3 position, GameObject[] objs, SpawnFunction create, LayerMask mask)
	{
		if(amount<=0)
			return;

		float clusterDistSqrd = clusterDist*clusterDist;
		int created = 0;

		do{
			Vector3 spawnPoint = new Vector3(UnityEngine.Random.Range(-radius,radius)+position.x, 0f, UnityEngine.Random.Range(-radius,radius)+position.z);
			spawnPoint.x = Mathf.Clamp(spawnPoint.x, -GenerateLevel.xx, GenerateLevel.xx);
			spawnPoint.z = Mathf.Clamp(spawnPoint.z, -GenerateLevel.zz, GenerateLevel.zz);
			Vector3 nearestLoc;

			if(!Physics.CheckSphere(spawnPoint, 2, mask))
			{
				if(objs[0] == null)
				{
					objs[created] = create(spawnPoint);
					NetworkServer.Spawn(objs[created]);
					created++;
				}else
				{
					nearestLoc = NearestTarget(objs, spawnPoint);
					if((nearestLoc-spawnPoint).sqrMagnitude>clusterDistSqrd)
					{
						objs[created] = create(spawnPoint);
						NetworkServer.Spawn(objs[created]);
						created++;
					}
				}
			}
		}while(created<amount);
	}

	public static Vector3 NearestTarget(GameObject[] Objects, Vector3 targetLoc)
	{
		float nearestDist, newDist;
		GameObject obj = null;

		if(Objects.Length>0)
		{
			nearestDist = (Objects[0].transform.position-targetLoc).sqrMagnitude;
			foreach(GameObject o in Objects)
			{
				if(o!=null)
				{
					newDist = (o.transform.position-targetLoc).sqrMagnitude;
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
