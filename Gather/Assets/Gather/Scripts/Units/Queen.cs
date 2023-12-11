using System.Collections.Generic;
using UnityEngine;
using Gather.AI;

namespace gather
{
    public class Queen : Unit
    {
        public LocationEvent redFlag;
        public LocationEvent greenFlag;
        //public GameEvent QueenMove;
        public AIController_Interface AIController;
        public SpawnConfig spawnConfig;
        DroneFactory droneFactory;
        Blackboard context = new Blackboard();
        [SerializeField] int foodQueueSize = 10;
        [SerializeField] int foodReserve = 5;
        Queue<Vector2> foodLocations;
        Counter foodCounter;
        float timer;

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
            teamConfig.SetUnitCount(TeamConfig.UnitType.Queen, 1);
            context.SetValue(Configs.FoodCounter, foodCounter);
            context.SetValue(Configs.TeamConfig, teamConfig);
            context.SetValue(Configs.FoodLocations, foodLocations);
            AIController = new QueenFSMController(this, context);
            AIController.Enable(GetTeam());
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if(timer >= 0.125)
            {
                timer = 0;
                AIController.AssessSituation();
            }
        }

        private void OnDisable()
        {
            teamConfig.SetUnitCount(TeamConfig.UnitType.Queen, -1);
        }

        public void SpawnFarmer()
        {
            if (foodCounter.amount >= spawnConfig.farmerCost)
            {
                FarmerDrone farmer = droneFactory.SpawnDrone<FarmerDrone>(myTransform.position).GetComponent<FarmerDrone>();
                farmer.SetQueen(this);
                farmer.SetTeam(teamConfig);

                teamConfig.SetUnitCount(TeamConfig.UnitType.Farmer, 1);
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

                teamConfig.SetUnitCount(TeamConfig.UnitType.Fighter, 1);
                foodCounter.AddAmount(-spawnConfig.fighterCost);
            }
        }

        public void Gather(Vector2 fromLocation)
        {
            foodCounter.AddAmount(1);
            if (!foodLocations.Contains(fromLocation) && foodLocations.Count < foodQueueSize)
            {
                foodLocations.Enqueue(fromLocation);
            }
            else
            {
                foodLocations.Dequeue();
                foodLocations.Enqueue(fromLocation);
            }
        }

        public Blackboard GetBlackboard()
        {
            return context;
        }
    }
}
