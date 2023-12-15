using System.Collections;
using UnityEngine;
using Gather.AI;

namespace gather
{
    public class FighterDrone : Drone
    {
        Blackboard context = new Blackboard();
        UnitType[] enemyTypes = {UnitType.Farmer, UnitType.Fighter};


        protected override void Awake()
        {
            base.Awake();
            //enemyDetector.SetEnemyType(unit => unit is Drone);
            enemyDetector.SetEnemyTypes(enemyTypes);

            context.SetValue(Configs.EnemyDetector, enemyDetector);
            context.SetValue(Configs.EnemySearchConfig, enemySearchConfig);
            fsmController = new FighterFSM_Controller(this, context);
        }

        private void OnDrawGizmosSelected()
        {
            //Gizmos.DrawSphere(transform.position, enemySearchConfig.searchDist);
        }

        public override void SetTeam(TeamConfig config)
        {
            base.SetTeam(config);
            Enable();
        }

        private void Enable()
        {
            fsmController.Enable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            context.Clear();
        }

        public override void SetQueen(Queen queenie)
        {
            base.SetQueen(queenie);
            queenie.redFlag += SetDestination;
        }
    }
}
