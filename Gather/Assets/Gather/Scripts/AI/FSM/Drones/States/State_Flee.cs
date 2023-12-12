using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Flee : FSM_State
    {
        List<Unit> enemies;
        Drone drone;
        SearchConfig config;

        public State_Flee(Drone drone, Blackboard context)
        {
            this.drone = drone;
            this.config = context.GetValue<SearchConfig>(Configs.SearchConfig);
        }

        public override void EnterState()
        {
            //Debug.Log("Fleeing");
            Flee();
        }

        public override void Update()
        {
            Flee();
        }

        public override void ExitState()
        {
            drone.StopAllCoroutines();
        }

        public override string GetStateName()
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

        void DebugDrawVector(Vector2 direction)
        {
            Debug.DrawRay(drone.Location(), drone.Location() - direction);
        }

        private List<Unit> DetectEnemies()
        {
            return TargetSystem.FindTargetsByCount<Unit>(
                config.searchAmount, config.searchTag, drone.Location(), config.searchDist, config.searchLayer, f => f.GetType() == typeof(FighterDrone) && f.GetTeam() != drone.GetTeam());
        }
        private Vector2 DangerZone(List<Unit> enemies)
        {
            Vector2 dangerZone = enemies[0].Location();
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
