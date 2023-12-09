using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Flee : IBehaviorState
    {
        List<Unit> enemies;
        Drone drone;
        SearchConfig config;

        public State_Flee(Drone drone, Blackboard context)
        {
            this.drone = drone;
            this.config = context.GetValue<SearchConfig>(Configs.SearchConfig);
        }
        public void AssesSituation()
        {
            Flee();
        }

        public void EnterState()
        {
            //Debug.Log("Fleeing");
            Flee();
        }

        public void ExitState()
        {
            drone.StopAllCoroutines();
        }

        string IBehaviorState.ToString()
        {
            return States.flee;
        }

        public void SetEnemiesList(List<Unit> enemies)
        {
            this.enemies = enemies;
        }

        void Flee()
        {
            enemies = DetectEnemies();
           
            if (enemies.Count > 0)
            {
                //DebugDrawVector(DangerZone(enemies));
                drone.SetDestination(drone.Location() + (drone.Location() - DangerZone(enemies)) );
                drone.StartCoroutine(RunFromTarget());
            }
        }

        void DebugDrawVector(Vector3 direction)
        {
            Debug.DrawRay(drone.Location(), drone.Location() - direction);
        }

        private List<Unit> DetectEnemies()
        {
            return TargetSystem.FindTargetsByCount<Unit>(
                config.searchAmount, config.searchTag, drone.Location(), config.searchDist, config.searchLayer, f => f.GetType() == typeof(FighterDrone) && f.GetTeam() != drone.GetTeam());
        }
        private Vector3 DangerZone(List<Unit> enemies)
        {
            Vector3 dangerZone = enemies[0].Location();
            for (int e = 1; e < enemies.Count; e++)
            {
                dangerZone += enemies[e].Location();
            }
            dangerZone /= enemies.Count;
            return dangerZone;
        }

        public IEnumerator RunFromTarget()
        {
            while (enemies.Count > 0 && Vector3.Distance(drone.Location(), DangerZone(enemies)) < config.searchDist)
            {
                //enemies = DetectEnemies();

                drone.SetDestination(drone.Location() + drone.Location() - DangerZone(enemies));
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
