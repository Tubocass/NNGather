using System.Collections;
using UnityEngine;
using Gather.AI;

namespace gather
{
    public class FighterDrone : Drone
    {
        [SerializeField] SearchConfig enemySearchConfig;
        Blackboard context = new Blackboard();

        protected override void Awake()
        {
            base.Awake();
            context.SetValue(Configs.EnemySearchConfig, enemySearchConfig);
            AIController = new FighterFSM_Controller(this, context);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, enemySearchConfig.searchDist);
        }

        public override void SetTeam(TeamConfig config)
        {
            base.SetTeam(config);
            Enable();
        }

        private void Enable()
        {
            AIController.Enable(GetTeam());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            teamConfig.SetUnitCount(TeamConfig.UnitType.Fighter, -1);
            context.Clear();
        }

        public override void SetQueen(Queen queenie)
        {
            base.SetQueen(queenie);
            queenie.redFlag += SetDestination;
        }
    }
}
