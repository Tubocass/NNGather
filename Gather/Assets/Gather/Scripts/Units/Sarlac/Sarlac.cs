using System.Collections;
using UnityEngine;

namespace gather
{
    public class Sarlac : Unit
    {
        bool isAwake = false;
        public bool isNight = false;
        [SerializeField] float orbitRadius = 40;
        Transform homePit;

        void Start()
        {
            TimeManager timeManager = FindFirstObjectByType<TimeManager>();
            timeManager.OnDusk.AddListener(SunDown);
            timeManager.OnDawn.AddListener(SunUp);
        }

        public void SetHome(Transform home)
        {
            this.homePit = home;
        }

        public bool IsAtHome()
        {
            return Vector2.Distance(GetLocation(), homePit.position) < 1f;
        }

        public void ReturnToHome()
        {
            SetDestination(homePit.position);
        }

        void SunUp()
        {
            isNight = false;
        }

        void SunDown()
        {
            isNight = true;
            isAwake = true;
        }

        public override void SetTeam(TeamConfig config)
        {
            this.teamConfig = config;
            teamConfig.UnitManager.UpdateUnitCount(unitType, 1);
            enemyDetector.SetTeam(teamConfig.TeamID);
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
