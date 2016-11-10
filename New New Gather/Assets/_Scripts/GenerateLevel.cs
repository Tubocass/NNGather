using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateLevel : MonoBehaviour 
{
	public GameObject Ground, NightPlant, DayPlant, Sarlac_Pit, EnemyMoMFab, MainMoMFab, GlowFab;
	public Vector3 groundSize;
	public int plants = 5, dayPatches = 5, pits = 3, plantClusterDist = 5, dayPlantPitDistance = 15, spClusterDist = 20, moms = 2, momsDistance = 20;
	public Color[] Colors;
	static Transform DayLight, NightLight;
	[SerializeField] float SunSpeed = 2f;
	GameObject[] Pits;
	GameObject[] MoMs;
	//List<FoodSpawner> PlantList = new List<FoodSpawner>();
	GameObject spawn;
	Vector3 spawnPoint;
	float plantClustSqrd, spClusterSqrd, mmDistanceSqrd, dpDistanceSqrd;
	float xx, zz;
	bool bDay;

	void Start () 
	{
		plantClustSqrd = plantClusterDist*plantClusterDist; //causing problems with lvl generation
		spClusterSqrd = spClusterDist*spClusterDist;
		mmDistanceSqrd = momsDistance * momsDistance;
		dpDistanceSqrd = dayPlantPitDistance * dayPlantPitDistance;
		groundSize = Ground.GetComponent<MeshRenderer>().bounds.extents;
		xx = groundSize.x - groundSize.x/8;
		zz = groundSize.z- groundSize.z/8;
		DayLight = GameObject.Find("Day Light").transform;
		NightLight = GameObject.Find("Night Light").transform;
		Pits = new GameObject[pits];
		MoMs = new GameObject[moms];
		int sp = 0, mm = 0, dp = 0;
		//Sarlac Pits
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
		//Day Plants
		do{
			spawnPoint = new Vector3(Random.Range(-xx,xx), 0.5f, Random.Range(-zz,zz));
			Vector3 nearestLoc = NearestTarget(Pits, spawnPoint);//Find(l=> (l.Location-spawnPoint).sqrMagnitude<clusterDist)
			if((nearestLoc-spawnPoint).sqrMagnitude>dpDistanceSqrd)
			{
				SpawnDayPlants(spawnPoint);
				dp++;
			}else{

				spawnPoint = new Vector3(Random.Range(-xx,xx), 0.5f, Random.Range(-zz,zz));
				nearestLoc = NearestTarget(Pits, spawnPoint);
				if((nearestLoc-spawnPoint).sqrMagnitude>dpDistanceSqrd)
				{
					SpawnDayPlants(spawnPoint);
					dp++;
				}
			}
			
		}while(dp<dayPatches);
		//MoMs
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

//	void OnGUI()
//	{
//		Vector3 screenPoint = Camera.main.WorldToScreenPoint(groundSize);
//		Graphics.DrawTexture(new Rect(0,0, groundSize.x*2, groundSize.z*2), DayTexture);
//	}

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
		int g = Random.Range(0,3);// number of glow rocks
		plants = Random.Range(3,7);// number of plants
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
				flowers[pl] = Instantiate(NightPlant, spawnPoint, Quaternion.identity)as GameObject;
				pl++;
				if(g>0)
				{
					GameObject glow = Instantiate(GlowFab, new Vector3(Random.Range(-plantClustSqrd,plantClustSqrd)+position.x, 0.5f, Random.Range(-plantClustSqrd,plantClustSqrd)+position.z), Quaternion.identity)as GameObject;
					g--;
				}
			}else
			{
				nearestLoc = NearestTarget(flowers, spawnPoint);//Find(l=> (l.Location-spawnPoint).sqrMagnitude<clusterDist)
				if((nearestLoc-spawnPoint).sqrMagnitude>plantClusterDist)
				{
					flowers[pl] = Instantiate(NightPlant, spawnPoint, Quaternion.identity)as GameObject;
					pl++;
					if(g>0)
					{
						GameObject glow =Instantiate(GlowFab, new Vector3(Random.Range(-plantClustSqrd,plantClustSqrd)+position.x, 0.5f, Random.Range(-plantClustSqrd,plantClustSqrd)+position.z), Quaternion.identity)as GameObject;
						g--;
					}
				}else{

					spawnPoint = new Vector3(Random.Range(-plantClustSqrd,plantClustSqrd)+position.x, 0.5f, Random.Range(-plantClustSqrd,plantClustSqrd)+position.z);
					Mathf.Clamp(spawnPoint.x, -xx, xx);
					Mathf.Clamp(spawnPoint.z, -zz, zz);
					nearestLoc = NearestTarget(flowers, spawnPoint);
					if((nearestLoc-spawnPoint).sqrMagnitude>plantClusterDist)
					{
						flowers[pl] = Instantiate(NightPlant, spawnPoint, Quaternion.identity)as GameObject;
						pl++;
						if(g>0)
						{
							GameObject glow =Instantiate(GlowFab, new Vector3(Random.Range(-plantClustSqrd,plantClustSqrd)+position.x, 0.5f, Random.Range(-plantClustSqrd,plantClustSqrd)+position.z), Quaternion.identity)as GameObject;
							g--;
						}
					}
				}
			}
		}while(pl<plants);
		return newPit;
	}
	GameObject SpawnDayPlants(Vector3 position)
	{
		GameObject dayPlant = Instantiate(DayPlant, position, Quaternion.identity)as GameObject;

		return dayPlant;
		
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
