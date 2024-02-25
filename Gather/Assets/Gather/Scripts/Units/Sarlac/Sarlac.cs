using UnityEngine;

namespace gather
{
    public class Sarlac : Unit, IRoamer
    {
        public bool isNight = false;
        [SerializeField] float orbitRadius = 40;
        Transform homePit;
        Collider2D[] myColliders;

        protected override void Awake()
        {
            base.Awake();
            TimeManager timeManager = FindFirstObjectByType<TimeManager>();
            timeManager.OnDusk.AddListener(SunDown);
            timeManager.OnDawn.AddListener(SunUp);
            myColliders = GetComponentsInChildren<Collider2D>();
        }

        void SunUp()
        {
            context.SetValue(Configs.IsNight, false);
        }

        void SunDown()
        {
            context.SetValue(Configs.IsNight, true);
        }

        public void Sleep()
        {
            spriteRenderer.enabled = false;
            for (int i = 0; i < myColliders.Length; i++)
            {
                myColliders[i].enabled = false;
            }
        }

        public void WakeUp()
        {
            spriteRenderer.enabled = true;
            for (int i = 0; i < myColliders.Length; i++)
            {
                myColliders[i].enabled = true;
            }
        }

        public override void SetTeam(TeamConfig config)
        {
            this.teamConfig = config;
            teamConfig.UnitManager.UpdateUnitCount(unitType, 1);
            enemyDetector.SetTeam(teamConfig.TeamID);
        }

        public void SetHome(Transform home)
        {
            this.homePit = home;
        }

        public bool IsAtHome()
        {
            return Vector2.Distance(GetLocation() + navAgent.centerOffset, homePit.position) < 1f;
        }

        public void ReturnHome()
        {
            SetDestination(homePit.position);
        }

        public void MoveRandomly(Vector2 center)
        {
            Vector2 direction = center + Random.insideUnitCircle * orbitRadius;
            SetDestination(direction);
        }

        public virtual Vector2 AnchorPoint()
        {
            return homePit.position;
        }
    }
}
