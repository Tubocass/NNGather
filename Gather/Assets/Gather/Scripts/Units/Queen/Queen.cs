using System.Collections;
using UnityEngine;

namespace Gather
{
    [RequireComponent(typeof(QueenFoodManager))]
    public class Queen : Unit
    {
        [SerializeField] float hungerTime;
        Anchor foodAnchor, fightAnchor;
        public DroneSpawnConfig spawnConfig;
        PrefabFactory farmerFactory, fighterFactory;
        QueenFoodManager foodManager;

        protected override void Awake()
        {
            base.Awake();
            foodManager = GetComponent<QueenFoodManager>();

            //foodCounter = ScriptableObject.CreateInstance<FoodCounter>();
            fighterFactory = GameObject.FindGameObjectWithTag(Tags.gameController)
                .GetComponent<FighterFactory>();
            farmerFactory = GameObject.FindGameObjectWithTag(Tags.gameController)
                .GetComponent<FarmerFactory>();

            //context.SetValue(Configs.FoodCounter, foodCounter);
            context.SetValue(Keys.SpawnConfig, spawnConfig);
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

        public void SetAnchors(Anchor foodAnchor, Anchor fightAnchor)
        {
            this.foodAnchor = foodAnchor;
            this.fightAnchor = fightAnchor;
            foodAnchor.transform.SetParent(myTransform, false);
            fightAnchor.transform.SetParent(myTransform, false);
        }

        public override void SetTeam(TeamConfig config)
        {
            base.SetTeam(config);
            context.SetValue(Keys.TeamConfig, teamConfig);
        }

        public void SpawnFarmer()
        {
            if (foodManager.Amount >= spawnConfig.farmerCost)
            {
                FarmerDrone farmer = farmerFactory.Spawn(myTransform.position)
                    .GetComponent<FarmerDrone>();
                farmer.SetQueen(this);
                farmer.SetAnchor(foodAnchor);
                farmer.SetTeam(teamConfig);

                foodManager.AddAmount(-spawnConfig.farmerCost);
            }
        }

        public void SpawnFighter()
        {
            if (foodManager.Amount >= spawnConfig.fighterCost)
            {
                FighterDrone fighter = fighterFactory.Spawn(myTransform.position)
                    .GetComponent<FighterDrone>();
                fighter.SetTeam(teamConfig);
                fighter.SetAnchor(fightAnchor);
                fighter.SetQueen(this);

                foodManager.AddAmount(-spawnConfig.fighterCost);
            }
        }

        public void Gather(Vector2 fromLocation)
        {
            foodManager.Gather(fromLocation);
            teamConfig.FoodManager.AddFoodSource(fromLocation);
        }

        IEnumerator Hunger()
        {
            while (isActiveAndEnabled)
            {
                yield return new WaitForSeconds(hungerTime);

                if (foodManager.Amount > 0)
                {
                    foodManager.AddAmount(-1);
                    health.Heal(1);
                }else
                {
                    health.TakeDamage(1);
                }
            }
        }

        public MaxCounter GetFoodCounter() 
        {
            return foodManager.GetCounter();
        }
    }
}
