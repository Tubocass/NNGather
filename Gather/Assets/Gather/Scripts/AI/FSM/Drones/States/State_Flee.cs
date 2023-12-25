using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Flee : FSM_State
    {
        List<Unit> enemies;
        Drone drone;
        EnemyDetector enemyDetector;

        public State_Flee(FarmerDrone drone, Blackboard context)
        {
            this.drone = drone;
            enemyDetector = context.GetValue<EnemyDetector>(Configs.EnemyDetector);
        }

        public override void EnterState()
        {
        }

        public override void Update()
        {
            enemies = enemyDetector.GetEnemiesList();

            if (enemies.Count > 0)
            {
                drone.SetDestination(drone.Location() + (drone.Location() - DangerZone(enemies)));
            }
        }

        public override void ExitState()
        {
        }

        private Vector2 DangerZone(List<Unit> enemies)
        {
            Vector2 dangerZone = enemies[0].Location();
            for (int e = 1; e < enemies.Count; e++)
            {
                dangerZone += enemies[e].Location();
            }
            dangerZone /= enemies.Count;
            return dangerZone;
        }
    }
}
