using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class EnemyDetector : MonoBehaviour
    {
        [SerializeField] SearchConfig config;
        [SerializeField] UnitType[] enemyTypes;
        List<Unit> enemies = new List<Unit>();
        Unit unitController;
        int team;
        bool detectedThing = false;
        public bool DetectedThing => detectedThing;

        private void Awake()
        {
            unitController = GetComponent<Unit>();
        }

        public void SetTeam(int team)
        {
            this.team = team;
        }

        public void Detect()
        {
            TargetSystem.FindTargetsByCount<Unit>(
                config.searchAmount, config.searchTag, unitController.GetLocation(), config.searchDist, config.searchLayer, f => f.CanBeTargeted(team) && ContainsUnitType(f.UnitType), out enemies
                );
            detectedThing = enemies.Count > 0;
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
