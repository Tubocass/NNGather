using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateLevel : MonoBehaviour 
{
	public GameObject Ground, Plant;
	public Vector3 groundSize;
	public int plants = 5, clusterDist = 5;
	List<FoodSpawner> PlantList = new List<FoodSpawner>();

	void Start () 
	{
		clusterDist *= clusterDist; 
		groundSize = Ground.GetComponent<MeshRenderer>().bounds.extents;
		float xx = groundSize.x - groundSize.x/8, zz = groundSize.z- groundSize.z/8;
		Vector3 spawnPoint;
		int pl = plants;
		do{
			spawnPoint = new Vector3(Random.Range(-xx,xx), 0.5f, Random.Range(-zz,zz));

			if(PlantList.Count<1)
			{
				GameObject spawn = Instantiate(Plant, spawnPoint, Quaternion.identity)as GameObject;
				FoodSpawner fd = spawn.GetComponent<FoodSpawner>();
				PlantList.Add(fd);
				pl--;
			}else
			{
				Vector3 nearestLoc = PlantList[0].Location;//Find(l=> (l.Location-spawnPoint).sqrMagnitude<clusterDist)
				if((nearestLoc-spawnPoint).sqrMagnitude>clusterDist)
				{
					GameObject spawn = Instantiate(Plant, spawnPoint, Quaternion.identity)as GameObject;
					FoodSpawner fd = spawn.GetComponent<FoodSpawner>();
					PlantList.Add(fd);
					pl--;
				}else{

					spawnPoint = new Vector3(Random.Range(-xx,xx), 0.5f, Random.Range(-zz,zz));
					if((nearestLoc-spawnPoint).sqrMagnitude>clusterDist)
					{
						GameObject spawn = Instantiate(Plant, spawnPoint, Quaternion.identity)as GameObject;
						FoodSpawner fd = spawn.GetComponent<FoodSpawner>();
						PlantList.Add(fd);
						pl--;
					}
				}
			}
		}while(pl>0);
	}

}
