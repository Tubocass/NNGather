using System.Collections.Generic;
using gather;

namespace Gather.AI.FSM.States
{
    public class DroneState_Hunt : FSM_State
    {
        List<Unit> enemies = new List<Unit>();
        Drone drone;
        Unit target;
        EnemyDetector enemyDetector;
        bool changePath;

        public DroneState_Hunt(Blackboard context) : base(context)
        {
            this.drone = context.GetValue<Drone>(Configs.Unit);
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

            if (enemyDetector.DetectedThing)
            {
                target = TargetSystem.TargetNearest(drone.GetLocation(), enemyDetector.GetEnemiesList());
                context.SetValue<ITargetable>(Configs.Target, target);
                drone.SetHasTarget(true);
            }
            else if(!drone.IsMoving || changePath)
            {
                changePath = false;
                drone.MoveRandomly(drone.AnchorPoint());
            }
        }
    }
}
