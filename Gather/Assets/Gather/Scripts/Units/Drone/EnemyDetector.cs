using System;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class EnemyDetector : MonoBehaviour
    {
        List<Unit> enemies = new List<Unit>();
        public StatusEvent EnemyDetected;
        Predicate<Unit> enemyCheck = unit => unit.isActiveAndEnabled;
        int team;
        CircleCollider2D searchCollider;
        private void Awake()
        {
            searchCollider = GetComponent<CircleCollider2D>();
        }

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

        public void SetRadius(float size)
        {
            searchCollider = GetComponent<CircleCollider2D>();

            searchCollider.radius = size;
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
                //Debug.Log("EnemyDetected");
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
