using System.Collections.Generic;
using gather;
using UnityEngine;

namespace Gather.AI
{
    public class State_Hunt : FSM_State
    {
        List<Unit> enemies = new List<Unit>();
        FighterDrone drone;
        Unit target;
        Blackboard context;
        EnemyDetector enemyDetector;


        public State_Hunt(FighterDrone fighter, Blackboard bb)
        {
            drone = fighter;
            context = bb;
            enemyDetector = context.GetValue<EnemyDetector>(Configs.EnemyDetector);
        }

        public void Clear()
        {
            target = null;
        }
        
        public override void EnterState()
        {
            Hunt();
        }

        public override void Update()
        {
            if (target && !target.CanTarget(drone.GetTeam()))
            {
                target = null;
            }
            if(target == null)
            {
                Hunt();
            } 
            else
            {
                drone.SetDestination(target.Location());
            }
        }

        public override void ExitState()
        {
            Clear();
        }

        public override string GetStateName()
        {
            return States.hunt;
        }

        public void SetEnemiesList(List<Unit> enemies)
        {
            this.enemies = enemies;
        }

        void Hunt()
        {
            //enemies = TargetSystem.FindTargetsByCount<Unit>(config.searchAmount, config.searchTag, drone.Location(), config.searchDist, config.searchLayer, e => e.CanTarget(drone.GetTeam()));
            enemyDetector.Detect();
            enemies = enemyDetector.GetEnemiesList();

            if (enemies.Count > 0)
            {
                target = TargetSystem.TargetNearest<Unit>(drone.Location(), enemies);
            }
            else if(!drone.IsMoving)
            {
                drone.MoveRandomly();
            }
        }
    }
}
