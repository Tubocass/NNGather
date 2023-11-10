using System;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class EnemyDetector : MonoBehaviour
    {
        List<Unit> enemies = new List<Unit>();
        public GameEvent EnemyDetected;
        public GameEvent AllClear;
        Predicate<Unit> enemyCheck = (unit => unit.isActiveAndEnabled);

        int team;

        public void SetTeam(int team)
        {
            this.team = team;
        }

        public void SetEnemyType(Predicate<Unit> unitType)
        {
            enemyCheck = unitType;
        }

        public List<Unit> GetEnemiesList()
        {
            return enemies;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(Tags.units))
            {
                return;
            }

            Unit enemy = collision.GetComponent<Unit>();
            if (enemyCheck(enemy) && enemy.GetTeam() != team)
            {
                if (!enemies.Contains(enemy))
                {
                    enemies.Add(enemy);
                }
                EnemyDetected?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag(Tags.units))
            {
                return;
            }

            Unit enemy = collision.GetComponent<Unit>();
            if (enemyCheck(enemy) && enemy.GetTeam() != team)
            {
                if (enemies.Contains(enemy))
                {
                    enemies.Remove(enemy);
                    if(enemies.Count == 0)
                    {
                        AllClear?.Invoke();
                    }
                }
            }
        }
    }
}
