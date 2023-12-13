using System;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class EnemyDetector : MonoBehaviour
    {
        List<Unit> enemies = new List<Unit>();
        public StatusEvent EnemyDetected;
        Predicate<Unit> enemyCheck;

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
                EnemyDetected?.Invoke(true);
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
                        EnemyDetected?.Invoke(false);
                    }
                }
            }
        }
    }
}
