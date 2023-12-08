using PolyNav;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class Queen : Unit
    {
        DroneFactory droneFactory;
        public LocationEvent redFlag;
        public LocationEvent greenFlag;
        public FoodEvent Collect;
        public GameEvent QueenMove;
        //[SerializeField] int food = 5;
        [SerializeField] int foodQueueSize = 10;
        Queue<Vector2> foodLocations;
        private int farmerCost = 1, fighterCost = 2;
        public Counter foodCounter;

        IEnumerator SpawnDrones()
        {
            while(true)
            {
                float spawnChance = Random.value;

                if(spawnChance > .8f)
                {
                    SpawnFighter();
                }else
                {
                    SpawnFarmer();
                }
                yield return new WaitForSeconds(2f);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            foodCounter = ScriptableObject.CreateInstance<Counter>();
            foodCounter.SetAmount(5);
        }

        protected void Start()
        {
            foodLocations = new Queue<Vector2>(foodQueueSize);
            droneFactory = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<DroneFactory>();
            SetTeamColor();
            Collect?.Invoke(foodCounter.amount);
            teamConfig.SetUnitCount(TeamConfig.UnitType.Queen, 1);
            StartCoroutine("SpawnDrones");
        }

        private void OnDisable()
        {
            teamConfig.SetUnitCount(TeamConfig.UnitType.Queen, -1);
        }

        public void SpawnFarmer()
        {
            if (foodCounter.amount >= farmerCost)
            {
                FarmerDrone farmer = droneFactory.SpawnDrone<FarmerDrone>(myTransform.position).GetComponent<FarmerDrone>();
                farmer.SetTeam(teamConfig);
                farmer.SetQueen(this);

                teamConfig.SetUnitCount(TeamConfig.UnitType.Farmer, 1);
                foodCounter.AddAmount(-farmerCost);
                Collect?.Invoke(-farmerCost);
            }
        }

        public void SpawnFighter()
        {
            if (foodCounter.amount >= fighterCost)
            {
                FighterDrone fighter = droneFactory.SpawnDrone<FighterDrone>(myTransform.position).GetComponent<FighterDrone>();
                fighter.SetTeam(teamConfig);
                fighter.SetQueen(this);

                teamConfig.SetUnitCount(TeamConfig.UnitType.Fighter, 1);
                foodCounter.AddAmount(-fighterCost);
                Collect?.Invoke(-fighterCost);
            }
        }

        public void Gather(Vector2 fromLocation)
        {
            Collect?.Invoke(1);
            foodCounter.AddAmount(1);
            if(foodLocations.Count < foodQueueSize)
            {
                foodLocations.Enqueue(fromLocation);
            }
            else
            {
                foodLocations.Dequeue();
                foodLocations.Enqueue(fromLocation);
                MovePosition();
            }
        }

        void MovePosition()
        {
            Vector2 newPosition =  Location();
            int size = foodLocations.Count;
            for (int np = size; np > 0; np--)
            {
                newPosition += foodLocations.Dequeue();
            }
            newPosition /= size;
            navAgent.SetDestination(newPosition);
            QueenMove?.Invoke();
        }
    }
}
