using gather;

namespace Gather.AI.FSM.States
{
    public class UnitState_Hunt : FSM_State
    {
        Unit drone;
        Unit target;
        EnemyDetector enemyDetector;

        public UnitState_Hunt(Blackboard context) : base(context)
        {
            this.drone = context.GetValue<Unit>(Configs.Unit);
            enemyDetector = context.GetValue<EnemyDetector>(Configs.EnemyDetector);
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
                target = TargetSystem.TargetNearest(drone.GetLocation(), enemyDetector.GetEnemiesList());
                context.SetValue<ITargetable>(Configs.Target, target);
                drone.SetHasTarget(true);
            }
        }
    }
}
