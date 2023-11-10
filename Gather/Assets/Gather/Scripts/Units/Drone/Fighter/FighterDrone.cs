using System.Collections;
using UnityEngine;
using Gather.AI;

namespace gather
{
    public class FighterDrone : Drone
    {
        [SerializeField] SearchConfig enemySearchConfig;
        State_Hunt huntState;
        State_Engage engageState;
        Blackboard context = new Blackboard();

        protected override void Awake()
        {
            base.Awake();

            huntState = new State_Hunt(this, context, enemySearchConfig);
            huntState.TargetFound += TargetFound;

            engageState = new State_Engage(this, context, enemySearchConfig);
            engageState.TargetLost += StartHunt;
            engageState.TargetReached += StartHunt;
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
            StartHunt();
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

        public void TargetFound()
        {
            BehaviorState = engageState;
        }

        public void StartHunt()
        {
            BehaviorState = huntState;
        }
    }
}
