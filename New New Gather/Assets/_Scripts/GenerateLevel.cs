using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GenerateLevel : MonoBehaviour 
{
	public GameObject Ground, NightPlantFab, DayPlantFab, Sarlac_PitFab, EnemyMoMFab, MainMoMFab, GlowFab;
	public Vector3 groundSize;
	public int plants = 5, dayScars = 5, pits = 3, plantClusterDist = 5, plantRadius = 15, dayPlantPitDistance = 15, spClusterDist = 20, moms = 2, momsDistance = 20;
	public Color[] Colors;
	static Transform DayLight, NightLight;
	[SerializeField] float SunSpeed = 2f;
	GameObject[] Pits;
	GameObject[] MoMs;
	GameObject spawn;
	Vector3 spawnPoint;
	float xx, zz;
	int MoMCount;
	bool bDay;
	private delegate GameObject SpawnFunction(Vector3 v);

	void Start () 
	{
		groundSize = Ground.GetComponent<MeshRenderer>().bounds.extents;
		xx = groundSize.x - groundSize.x/8;
		zz = groundSize.z- groundSize.z/8;
		DayLight = GameObject.Find("Day Light").transform;
		NightLight = GameObject.Find("Night Light").transform;
		Pits = new GameObject[pits];
		MoMs = new GameObject[moms];

		//Sarlac Pits
		SpawnObjects(pits, xx, spClusterDist, Vector3.zero, Pits, SpawnSarlacPit);

		//Day Plants
		SpawnObjects(dayScars, xx, dayPlantPitDistance, Vector3.zero, Pits, SpawnDayPlants);

		//MoMs
		SpawnObjects(moms, xx, momsDistance, Vector3.zero, MoMs, SpawnMoM);


	}
	void Update()
	{
		DayLight.Rotate(DayLight.right,SunSpeed*Time.deltaTime,Space.World);
		NightLight.Rotate(NightLight.right,SunSpeed*Time.deltaTime,Space.World);
		if(!IsDayLight()&&bDay)
		{
			bDay = false;
			UnityEventManager.TriggerEvent("DayTime",false);
			DayLight.gameObject.SetActive(false);
		}else if(IsDayLight()&&!bDay){
			bDay = true;
			UnityEventManager.TriggerEvent("DayTime",true);
			DayLight.gameObject.SetActive(true);
		}

	}
	public static bool IsDayLight()
	{
		return DayLight.eulerAngles.x>0-10&&DayLight.eulerAngles.x<180+10;
	}

	GameObject SpawnMoM(Vector3 position)
	{
		GameObject newMoM;
		if(MoMCount==0)
		{
			newMoM = Instantiate(MainMoMFab, spawnPoint, Quaternion.identity)as GameObject;
		}else {
			newMoM = Instantiate(EnemyMoMFab, position, Quaternion.identity)as GameObject;
		}
		newMoM.GetComponent<MoMController>().teamID = MoMCount;
		newMoM.GetComponent<MoMController>().TeamColor = Colors[MoMCount];
		newMoM.GetComponentInChildren<MeshRenderer>().material.color = Colors[MoMCount];
		MoMCount+=1;
		return newMoM;
	}
	GameObject SpawnSarlacPit(Vector3 position)
	{
		GameObject newPit =	Instantiate(Sarlac_PitFab, position, Quaternion.identity)as GameObject;
		PitController.Pits.Add(newPit.GetComponent<PitController>());
		int g = UnityEngine.Random.Range(0,3);// number of glow rocks
		plants = UnityEngine.Random.Range(3,7);// number of plants
		SpawnObjects(NightPlantFab, plants, plantRadius, plantClusterDist, position);
		if(g>0)
		SpawnObjects(GlowFab, g, 15, plantClusterDist, position);
		return newPit;
	}

	GameObject SpawnDayPlants(Vector3 position)
	{
		GameObject dayPlant = Instantiate(DayPlantFab, position, Quaternion.identity)as GameObject;
		return dayPlant;
	}

	void SpawnObjects(GameObject fab, int amount, float radius, float clusterDist, Vector3 position)
	{
		GameObject[] objs = new GameObject[amount]; 
		SpawnObjects(amount, radius, clusterDist, position, objs, (Vector3 pos)=>
		{
				GameObject obj = Instantiate(fab, pos, Quaternion.identity)as GameObject;
			return obj; 
			});
	}

	void SpawnObjects(int amount, float radius, float clusterDist, Vector3 position, GameObject[] objs, SpawnFunction create)//, LayerMask mask
	{
		//GameObject[] objs = new GameObject[amount]; 
		float clusterDistSqrd = clusterDist*clusterDist;
		int created = 0;
		do{
			spawnPoint = new Vector3(UnityEngine.Random.Range(-radius,radius)+position.x, 0.5f, UnityEngine.Random.Range(-radius,radius)+position.z);
			Mathf.Clamp(spawnPoint.x, -xx, xx);
			Mathf.Clamp(spawnPoint.z, -zz, zz);
			Vector3 nearestLoc;

			if(created<1)
			{
//				RaycastHit[] hits = Physics.SphereCastAll(position,.5f,Vector3.down,1,mask, QueryTriggerInteraction.Ignore);
//				if(hits.Length>0)
//				{
//				}
				objs[created] = create(spawnPoint);
				created++;
			}else
			{
				nearestLoc = NearestTarget(objs, spawnPoint);
				if((nearestLoc-spawnPoint).sqrMagnitude>clusterDistSqrd)
				{
					objs[created] = create(spawnPoint);
					created++;
				}else{

					spawnPoint = new Vector3(UnityEngine.Random.Range(-radius,radius)+position.x, 0.5f, UnityEngine.Random.Range(-radius,radius)+position.z);
					Mathf.Clamp(spawnPoint.x, -xx, xx);
					Mathf.Clamp(spawnPoint.z, -zz, zz);
					nearestLoc = NearestTarget(objs, spawnPoint);
					if((nearestLoc-spawnPoint).sqrMagnitude>clusterDistSqrd)
					{
						objs[created] = create(spawnPoint);
						created++;
					}
				}
			}
		}while(created<amount);
	}


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
