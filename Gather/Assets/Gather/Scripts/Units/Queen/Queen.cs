using System.Collections.Generic;
using UnityEngine;
using Gather.AI;

namespace gather
{
    public class Queen : Unit
    {
        public LocationEvent redFlag;
        public LocationEvent greenFlag;
        public SpawnConfig spawnConfig;
        DroneFactory droneFactory;
        [SerializeField] int foodQueueSize = 10;
        [SerializeField] int foodReserve = 5;
        [SerializeField] int startingFood = 5;
        [SerializeField] int maxFood = 20;
        Queue<Vector2> foodLocations;
        Counter foodCounter;

        protected override void Awake()
        {
            base.Awake();
            foodCounter = ScriptableObject.CreateInstance<Counter>();
            foodCounter.SetAmount(startingFood);
            foodLocations = new Queue<Vector2>(foodQueueSize);
            droneFactory = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<DroneFactory>();

            context.SetValue(Configs.FoodCounter, foodCounter);
            context.SetValue(Configs.FoodLocations, foodLocations);
            context.SetValue(Configs.SpawnConfig, spawnConfig);
        }

        protected virtual void Start()
        {
            fsmController = new QueenFSM_Controller(this, context);
        }

        public override void SetTeam(TeamConfig config)
        {
            base.SetTeam(config);
            context.SetValue(Configs.TeamConfig, teamConfig);
        }

        public void SpawnFarmer()
        {
            if (foodCounter.amount >= spawnConfig.farmerCost)
            {
                FarmerDrone farmer = droneFactory.SpawnDrone<FarmerDrone>(myTransform.position).GetComponent<FarmerDrone>();
                farmer.SetQueen(this);
                farmer.SetTeam(teamConfig);

                foodCounter.AddAmount(-spawnConfig.farmerCost);
            }
        }

        public void SpawnFighter()
        {
            if (foodCounter.amount >= spawnConfig.fighterCost)
            {
                FighterDrone fighter = droneFactory.SpawnDrone<FighterDrone>(myTransform.position).GetComponent<FighterDrone>();
                fighter.SetTeam(teamConfig);
                fighter.SetQueen(this);

                foodCounter.AddAmount(-spawnConfig.fighterCost);
            }
        }

        public void Gather(Vector2 fromLocation)
        {
            foodCounter.AddAmount(1);
            if (!foodLocations.Contains(fromLocation) )
            {
                if(foodLocations.Count < foodQueueSize)
                {
                    foodLocations.Enqueue(fromLocation);
                }
                else
                {
                    foodLocations.Dequeue();
                    foodLocations.Enqueue(fromLocation);
                }
            }
            
        }

        public float AverageDistanceFromFood()
        {
            Vector2 avgPos = Location();
            Vector2[] locations = foodLocations.ToArray();
            int size = foodLocations.Count + 1;
            for (int ap = 0; ap < locations.Length; ap++)
            {
                avgPos += locations[ap];
            }
           return Vector2.Distance(Location(), avgPos /= size);
        }

        public bool IsFoodLow()
        {
            return foodCounter.amount <= foodReserve;
        }

        public bool IsFoodFull()
        {
            return foodCounter.amount >= maxFood;
        }
    }
}
