using System.Collections;
using UnityEngine;

namespace gather
{
    public class Sarlac : Unit
    {
        bool isAwake = false;
        public bool isNight = false;
        [SerializeField] float orbitRadius = 40;
        [SerializeField] float refractoryTime = 1f;
        Transform homePit;
        bool canFire = true;

        void Start()
        {
            TimeManager timeManager = FindFirstObjectByType<TimeManager>();
            timeManager.OnDusk.AddListener(SunDown);
            timeManager.OnDawn.AddListener(SunUp);
        }

        public bool IsAtHome()
        {
            return Vector2.Distance(CurrentLocation(), homePit.position) < float.Epsilon;
        }

        public Transform GetHome()
        {
            return homePit;
        }

        public void SetHome(Transform home)
        {
            this.homePit = home;
        }

        void SunUp()
        {
            isNight = false;
            /*
             * if at home then sleep, else move to home
            */
        }

        void SunDown()
        {
            isNight = true;
            isAwake = true;
            /*
             * play sound and animation, start hunting
            */
        }

        public override void SetTeam(TeamConfig config)
        {
            this.teamConfig = config;
            teamConfig.UpdateUnitCount(unitType, 1);
            enemyDetector.SetTeam(teamConfig.Team);
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

        public void Attack(Unit other)
        {
            if (!canFire)
                return;

            other.Death(); // Change to TakeDamage(value)
            canFire = false;
            if (gameObject.activeSelf)
            {
                StartCoroutine(RefractoryPeriod());
            }
        }

        IEnumerator RefractoryPeriod()
        {
            yield return new WaitForSeconds(refractoryTime);
            canFire = true;
        }
    }
}
