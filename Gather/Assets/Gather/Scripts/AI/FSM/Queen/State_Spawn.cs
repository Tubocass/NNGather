using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Spawn : IBehaviorState
    {
        public GameEvent Finished;
        Queen queen;
        Blackboard context;
        //TeamConfig teamConfig;
        //private int farmerCost = 1, fighterCost = 2;
        private int farmerCap = 6, fighterCap = 3;
        private int farmers, fighters;

        public State_Spawn (Queen queen, Blackboard context)
        {
            this.queen = queen;
            this.context = context;
        }

        public void EnterState()
        {
            // spawn how many of what
            farmers = 0;
            fighters = 0;
            queen.StartCoroutine(SpawnDrones());
            //Debug.Log("Spawning");
        }

        public void AssesSituation()
        {
            
        }

        public void ExitState()
        {
            queen.StopCoroutine(SpawnDrones());
        }

        string IBehaviorState.ToString()
        {
            return States.spawn;
        }

        IEnumerator SpawnDrones()
        {
            while (farmers < farmerCap || fighters < fighterCap)
            {
                float spawnChance = Random.value;

                if (spawnChance > .66f)
                {
                    queen.SpawnFighter();
                    fighters++;
                }
                else
                {
                    queen.SpawnFarmer();
                    farmers++;
                }
                yield return new WaitForSeconds(1f);
            }
            Finished?.Invoke();
        }
    }
}