using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace gather
{
    public class Queen : Unit
    {
        public LocationEvent redFlag;
        public LocationEvent greenFlag;
        public SpawnConfig spawnConfig;
        DroneFactory droneFactory;
        FoodCounter foodCounter;
        Health health;
        [SerializeField] float hungerTime = 2f;

        [SerializeField] Transform foodAnchor, fightAnchor;
        bool foodAnchorActive, fightAnchorActive;
        public Transform FoodAnchor { get { return foodAnchorActive? foodAnchor : myTransform;  } }
        public Transform FightAnchor { get { return fightAnchorActive ? fightAnchor : myTransform; } }

        protected override void Awake()
        {
            base.Awake();
            health = GetComponent<Health>();
            foodCounter = ScriptableObject.CreateInstance<FoodCounter>();
            droneFactory = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<DroneFactory>();

            context.SetValue(Configs.FoodCounter, foodCounter);
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
            foodCounter.Gather(fromLocation);
        }

        IEnumerator  Hunger()
        {
            while (true)
            {
                yield return new WaitForSeconds(hungerTime);

                if (foodCounter.amount > 0)
                {
                    foodCounter.AddAmount(-1);
                    health.Heal(1);
                } else
                {
                    health.TakeDamage(1);
                }
            }
        }

        public void PlaceFoodAnchor(Vector2 location)
        {
            foodAnchorActive = true;
            foodAnchor.gameObject.SetActive(true);
            foodAnchor.position = location;
            greenFlag?.Invoke(location);
        }

        public void RemoveFoodAnchor()
        {
            foodAnchorActive = false;
            foodAnchor.gameObject.SetActive(false);
        }

        public void PlaceFightAnchor(Vector2 location)
        {
            fightAnchorActive = true;
            fightAnchor.gameObject.SetActive(true);
            fightAnchor.position = location;
            redFlag?.Invoke(location);
        }

        public void RemoveFightAnchor()
        {
            fightAnchorActive = false;
            fightAnchor.gameObject.SetActive(false);
        }
    }
}
