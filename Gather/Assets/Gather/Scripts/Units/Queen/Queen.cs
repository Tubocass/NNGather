using System.Collections;
using UnityEngine;

namespace gather
{
    public class Queen : Unit
    {
        public Anchor foodAnchor, fightAnchor;
        public DroneSpawnConfig spawnConfig;
        PrefabFactory farmerFactory, fighterFactory;
        FoodCounter foodCounter;
        Health health;
        InputManager inputManager;
        [SerializeField] float hungerTime;

        protected override void Awake()
        {
            base.Awake();
            health = GetComponent<Health>();
            foodCounter = ScriptableObject.CreateInstance<FoodCounter>();
            fighterFactory = GameObject.FindGameObjectWithTag(Tags.gameController)
                .GetComponent<FighterFactory>();
            farmerFactory = GameObject.FindGameObjectWithTag(Tags.gameController)
                .GetComponent<FarmerFactory>();

            //context.SetValue(Configs.FoodCounter, foodCounter);
            context.SetValue(Configs.SpawnConfig, spawnConfig);
        }

        protected void Start()
        {
            StartCoroutine(Hunger());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StopCoroutine(Hunger());
        }

        public void SetInputManager(InputManager input)
        {
            inputManager = input;
        }

        public override void SetTeam(TeamConfig config)
        {
            base.SetTeam(config);
            context.SetValue(Configs.TeamConfig, teamConfig);
        }

        public void SpawnFarmer()
        {
            if (foodCounter.Amount >= spawnConfig.farmerCost)
            {
                FarmerDrone farmer = farmerFactory.Spawn(myTransform.position)
                    .GetComponent<FarmerDrone>();
                farmer.SetQueen(this);
                farmer.SetTeam(teamConfig);

                foodCounter.AddAmount(-spawnConfig.farmerCost);
            }
        }

        public void SpawnFighter()
        {
            if (foodCounter.Amount >= spawnConfig.fighterCost)
            {
                FighterDrone fighter = fighterFactory.Spawn(myTransform.position)
                    .GetComponent<FighterDrone>();
                fighter.SetTeam(teamConfig);
                fighter.SetQueen(this);

                foodCounter.AddAmount(-spawnConfig.fighterCost);
            }
        }

        public void Gather(Vector2 fromLocation)
        {
            foodCounter.Gather(fromLocation);
        }

        IEnumerator Hunger()
        {
            while (Application.isPlaying)
            {
                yield return new WaitForSeconds(hungerTime);

                if (foodCounter.Amount > 0)
                {
                    foodCounter.AddAmount(-1);
                    health.Heal(1);
                }else
                {
                    health.TakeDamage(1);
                }
            }
        }

        public void PlaceFoodAnchor()
        {
            inputManager.SetActiveAnchor(foodAnchor);
            foodAnchor.SetReadyToPlace();
        }

        public void PlaceFightAnchor()
        {
            inputManager.SetActiveAnchor(fightAnchor);
            fightAnchor.SetReadyToPlace();
        }

        public FoodCounter GetFoodCounter() 
        { 
            return foodCounter; 
        }
    }
}
