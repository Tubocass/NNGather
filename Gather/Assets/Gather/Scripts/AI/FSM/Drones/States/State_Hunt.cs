using System.Collections.Generic;
using gather;

namespace Gather.AI
{
    public class State_Hunt : FSM_State
    {
        List<Unit> enemies = new List<Unit>();
        Drone drone;
        Unit target;
        Blackboard context;
        EnemyDetector enemyDetector;
        bool changePath;

        public State_Hunt(Drone fighter, Blackboard bb)
        {
            drone = fighter;
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
                target = TargetSystem.TargetNearest(drone.CurrentLocation(), enemies);
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
