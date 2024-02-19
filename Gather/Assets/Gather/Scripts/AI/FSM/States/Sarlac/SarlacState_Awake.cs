using gather;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class SarlacState_Awake : FSM_State
    {
        Sarlac sarlac;
        Unit target;
        EnemyDetector enemyDetector;
        bool changePath;

        public SarlacState_Awake(Sarlac sarlac)
        {
            this.sarlac = sarlac;
            enemyDetector = sarlac.Blackboard.GetValue<EnemyDetector>(Configs.EnemyDetector);
        }

        public override void EnterState()
        {
            changePath = true;
            Debug.Log("Awake");
        }

        public override void Update()
        {
            Hunt();
        }

        public override void ExitState()
        {
            target = null;
        }

        void Hunt()
        {
            enemyDetector.Detect();

            if (enemyDetector.DetectedThing)
            {
                target = TargetSystem.TargetNearest(sarlac.GetLocation(), enemyDetector.GetEnemiesList());
                sarlac.Blackboard.SetValue<ITargetable>(Configs.Target, target);
                sarlac.SetHasTarget(true);
            } else if (!sarlac.IsMoving || changePath)
            {
                changePath = false;
                sarlac.MoveRandomly(sarlac.AnchorPoint());
            }
        }
    }
}
