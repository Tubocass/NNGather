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

        public override string GetStateName()
        {
            return States.hunt;
        }

        void Hunt()
        {
            enemyDetector.Detect();
            enemies = enemyDetector.GetEnemiesList();

            if (enemies.Count > 0)
            {
                target = TargetSystem.TargetNearest(drone.Location(), enemies);
                context.SetValue<ITarget>(Configs.Target, target);
                drone.hasTarget = true;
            }
            else if(!drone.IsMoving || changePath)
            {
                changePath = false;
                drone.MoveRandomly();
            }
        }
    }
}
