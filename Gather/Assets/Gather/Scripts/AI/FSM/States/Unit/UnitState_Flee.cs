using System.Collections.Generic;
using UnityEngine;
using Gather;

namespace Gather.AI.FSM.States
{
    public class UnitState_Flee : FSM_State
    {
        Unit unit;
        EnemyDetector enemyDetector;

        public UnitState_Flee(Blackboard context) : base(context)
        {
            this.unit = context.GetValue<Unit>(Keys.Unit);
            enemyDetector = context.GetValue<EnemyDetector>(Keys.EnemyDetector);
        }

        public override void EnterState()
        {
        }

        public override void Update()
        {
            if (enemyDetector.DetectedThing)
            {
                unit.SetDestination(unit.GetLocation() + (unit.GetLocation() - DangerZone(enemyDetector.GetEnemiesList())));
            }
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
