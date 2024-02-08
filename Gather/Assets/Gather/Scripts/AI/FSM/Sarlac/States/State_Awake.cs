using gather;
using System.Collections.Generic;

namespace Gather.AI
{
    public class State_Awake : FSM_State
    {
        List<Unit> enemies = new List<Unit>();
        Sarlac sarlac;
        Unit target;
        Blackboard context;
        EnemyDetector enemyDetector;
        bool changePath;

        public State_Awake(Sarlac sarlac, Blackboard bb)
        {
            this.sarlac = sarlac;
            context = bb;
            enemyDetector = context.GetValue<EnemyDetector>(Configs.EnemyDetector);
        }

        public override void EnterState()
        {
            changePath = true;
        }

        public override void Update()
        {
            Hunt();
        }

        public override void ExitState()
        {
            target = null;
            enemies.Clear();
        }

        void Hunt()
        {
            enemyDetector.Detect();
            enemies = enemyDetector.GetEnemiesList();

            if (enemies.Count > 0)
            {
                target = TargetSystem.TargetNearest(sarlac.CurrentLocation(), enemies);
                context.SetValue<ITargetable>(Configs.Target, target);
                sarlac.SetHasTarget(true);
            } else if (!sarlac.IsMoving || changePath)
            {
                changePath = false;
                sarlac.MoveRandomly(sarlac.AnchorPoint());
            }
        }
    }
}
