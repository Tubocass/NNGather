using System;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class EnemyDetector : MonoBehaviour, IDetector
    {
        [SerializeField] SearchConfig config;
        [SerializeField] UnitType[] enemyTypes;
        List<Unit> enemies = new List<Unit>();
        Unit unitController;
        int team;

        private void Awake()
        {
            unitController = GetComponent<Unit>();
        }

        public void SetTeam(int team)
        {
            this.team = team;
        }

        public List<Unit> GetEnemiesList()
        {
            return enemies;
        }

        bool ContainsUnitType(UnitType type)
        {
            for(int t = 0; t< enemyTypes.Length; t++)
            {
                if (type == enemyTypes[t])
                {
                    return true;
                }
            }
            return false;
        }

        public bool Detect()
        {
            TargetSystem.FindTargetsByCount<Unit>(
                config.searchAmount, config.searchTag, unitController.CurrentLocation(), config.searchDist, config.searchLayer, f => f.CanTarget(team) && ContainsUnitType(f.GetUnitType()), out enemies
                );
            return enemies.Count > 0;
        }
    }
}
