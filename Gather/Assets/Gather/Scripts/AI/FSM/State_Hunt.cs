using System.Collections.Generic;
using gather;

namespace Gather.AI
{
    public class State_Hunt : IBehaviorState
    {
        List<Unit> enemies = new List<Unit>();
        FighterDrone drone;
        Unit target;
        SearchConfig config;
        Blackboard context;
        public event TargetEvent TargetFound;


        public State_Hunt(FighterDrone fighter, Blackboard bb)
        {
            drone = fighter;
            context = bb;
            config = context.GetValue<SearchConfig>(Configs.EnemySearchConfig);
        }

        public void Clear()
        {
            enemies.Clear();
            target = null;
        }
        
        public void EnterState()
        {
            //Debug.Log("Hunting");
            Hunt();
        }

        public void AssesSituation()
        {
            if (target && !target.CanTarget(drone.GetTeam()))
            {
                target = null;
            }
            if(target == null)
            {
                Hunt();
            }
        }

        public void ExitState()
        {
            Clear();
        }

        string IBehaviorState.ToString()
        {
            return States.hunt;
        }

        public void SetEnemiesList(List<Unit> enemies)
        {
            this.enemies = enemies;
        }

        void Hunt()
        {
            enemies = TargetSystem.FindTargetsByCount<Unit>(config.searchAmount, config.searchTag, drone.Location(), config.searchDist, config.searchLayer, e => e.CanTarget(drone.GetTeam()));

            if (enemies.Count > 0)
            {
                target = TargetSystem.TargetNearest<Unit>(drone.Location(), enemies);
                context.SetValue<ITarget>(Configs.Target, target);
                TargetFound?.Invoke();
            }
            else
            {
                drone.MoveRandomly();
            }
        }
    }
}
