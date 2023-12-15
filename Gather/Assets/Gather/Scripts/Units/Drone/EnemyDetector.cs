using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace gather
{
    public class EnemyDetector
    {
        [SerializeField] SearchConfig config;
        Unit unitController;
        List<Unit> enemies = new List<Unit>();
        UnitType[] enemyTypes = new UnitType[1];
        int team;
        //Predicate<Unit> enemyCheck = unit => unit.isActiveAndEnabled;

        public EnemyDetector(Unit unitController, SearchConfig config)
        {
            this.unitController = unitController;
            this.config = config;
        }

        public void SetTeam(int team)
        {
            this.team = team;
        }

        //public void SetEnemyType(Predicate<Unit> unitType)
        //{
        //    enemyCheck = unitType;
        //}

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

        public bool Detect()
        {
            enemies = TargetSystem.FindTargetsByCount<Unit>(
                config.searchAmount, config.searchTag, unitController.Location(), config.searchDist, config.searchLayer, f => enemyTypes.Contains(f.GetUnitType()) && f.GetTeam() != team);
            return enemies.Count > 0;
        }
    }
}
