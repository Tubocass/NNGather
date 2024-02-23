using gather;

namespace Gather.AI.FSM.States
{
    public class SarlacState_Awake : FSM_State
    {
        Sarlac sarlac;
        Unit target;
        EnemyDetector enemyDetector;
        bool changePath;

        public SarlacState_Awake(Blackboard context) : base(context)
        {
            this.sarlac = context.GetValue<Sarlac>(Configs.Unit);
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
