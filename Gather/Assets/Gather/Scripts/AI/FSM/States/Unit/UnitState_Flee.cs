using System.Collections.Generic;
using UnityEngine;
using gather;

namespace Gather.AI.FSM.States
{
    public class UnitState_Flee : FSM_State
    {
        List<Unit> enemies;
        Unit unit;
        EnemyDetector enemyDetector;

        public UnitState_Flee(Blackboard context) : base(context)
        {
            this.unit = context.GetValue<Unit>(Configs.Unit);
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
                unit.SetDestination(unit.GetLocation() + (unit.GetLocation() - DangerZone(enemies)));
            }
        }

        public override void ExitState()
        {
        }

        private Vector2 DangerZone(List<Unit> enemies)
        {
            Vector2 dangerZone = enemies[0].GetLocation();
            for (int e = 1; e < enemies.Count; e++)
            {
                dangerZone += enemies[e].GetLocation();
            }
            dangerZone /= enemies.Count;
            return dangerZone;
        }
    }
}
