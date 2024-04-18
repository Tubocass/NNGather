using System.Collections.Generic;
using UnityEngine;
using System;


namespace Gather
{
    public class EnemyDetector : MonoBehaviour
    {
        [SerializeField] SearchConfig config;
        [SerializeField] UnitType[] enemyTypes;
        List<Unit> enemies = new List<Unit>();
        Unit unitController;
        Predicate<Unit> canTarget;
        int team;
        bool detectedThing = false;
        public bool DetectedThing => detectedThing;

        private void Awake()
        {
            unitController = GetComponent<Unit>();
            canTarget = CanTarget;
        }

        public void SetTeam(int team)
        {
            this.team = team;
        }

        public void Detect()
        {
            TargetSystem.FindTargetsByCount<Unit>(enemies, unitController.GetLocation(), config, canTarget);
            detectedThing = enemies != null && enemies.Count > 0;
        }

        bool CanTarget(Unit target)
        {
            return target.CanBeTargeted(team) && ContainsUnitType(target.UnitType);
        }

        bool ContainsUnitType(UnitType type)
        {
            for (int t = 0; t < enemyTypes.Length; t++)
            {
                if (type == enemyTypes[t])
                {
                    return true;
                }
            }
            return false;
        }

        public List<Unit> GetEnemiesList()
        {
            return enemies;
        }
    }
}
