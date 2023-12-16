using System;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class EnemyDetector
    {
        SearchConfig config;
        Unit unitController;
        List<Unit> enemies = new List<Unit>();
        UnitType[] enemyTypes = new UnitType[1];
        int team;

        public EnemyDetector(Unit unitController, SearchConfig config)
        {
            this.unitController = unitController;
            this.config = config;
        }

        public void SetTeam(int team)
        {
            this.team = team;
        }

        public void SetEnemyTypes(UnitType[] enemyTypes)
        {
            this.enemyTypes = enemyTypes; 
        }

        public List<Unit> GetEnemiesList()
        {
            return enemies;
        }

        public void SetSearchConfig(SearchConfig config)
        {
            this.config = config;
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
                config.searchAmount, config.searchTag, unitController.Location(), config.searchDist, config.searchLayer, f => ContainsUnitType(f.GetUnitType()) && f.CanTarget(team), out enemies
                );
            return enemies.Count > 0;
        }
    }
}
