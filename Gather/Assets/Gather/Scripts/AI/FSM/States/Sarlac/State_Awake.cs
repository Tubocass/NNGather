using gather;

namespace Gather.AI.FSM.States
{
    public class State_Awake : FSM_State
    {
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
        }

        void Hunt()
        {
            enemyDetector.Detect();

            if (enemyDetector.DetectedThing)
            {
                target = TargetSystem.TargetNearest(sarlac.GetLocation(), enemyDetector.GetEnemiesList());
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
