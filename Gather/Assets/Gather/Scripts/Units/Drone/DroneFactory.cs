using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class DroneFactory : MonoBehaviour
    {
        [SerializeField] GameObject farmerPrefab;
        [SerializeField] GameObject fighterPrefab;

        Dictionary<Type, List<GameObject>> pools = new Dictionary<Type, List<GameObject>>();

        private DroneFactory() { }

        private void Awake()
        {
            //List<GameObject> starterPool = new List<GameObject>();
            //GameObject drone;
            //TeamConfig tempTeam = ScriptableObject.CreateInstance<TeamConfig>();
            //for (int pool = 50; pool > 0; pool --)
            //{
            //    drone = Instantiate(farmerPrefab, Vector3.zero, Quaternion.identity);
            //    drone.GetComponent<FarmerDrone>().SetTeam(tempTeam);
            //    drone.SetActive(false);
            //    starterPool.Add(drone);
            //}
            //pools.Add(typeof(FarmerDrone), starterPool);
        }

        public GameObject SpawnDrone<T>(Vector3 location)
        {
            List<GameObject> drones;
            GameObject drone;
            GameObject prefab = typeof(T) == typeof(FarmerDrone) ? farmerPrefab : fighterPrefab;
            if (!prefab)
            {
                // throw error
                return null;
            }
            if(pools.TryGetValue(typeof(T), out drones))
            {
                if (drones.Count > 0)
                {
                    drone = drones.Find(f => !f.activeSelf);
                    if (drone == null)
                    {
                        drone = Instantiate(prefab, location, Quaternion.identity);
                        drones.Add(drone);
                    }
                    else
                    {
                        drone.transform.position = location;
                        drone.SetActive(true);
                    }
                    return drone;
                }
                else
                {
                    drone = Instantiate(prefab, location, Quaternion.identity);
                    drones.Add(drone);
                    return drone;
                }
            }
            else
            {
                drones = new List<GameObject>();
                drone = Instantiate(prefab, location, Quaternion.identity);
                drones.Add(drone);
                pools.Add(typeof(T), drones);
                return drone;
            }
        }
    }
}
