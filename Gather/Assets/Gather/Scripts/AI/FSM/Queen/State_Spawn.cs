using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Spawn : IBehaviorState
    {
        public GameEvent Finished;

        public State_Spawn (Queen quuen, Blackboard context)
        {
            // spawn how many of what
        }
        public void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public void AssesSituation()
        {
            throw new System.NotImplementedException();
        }

        public void ExitState()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator SpawnDrones()
        {
            while (true)
            {
                float spawnChance = Random.value;

                if (spawnChance > .8f)
                {
                    //SpawnFighter();
                }
                else
                {
                    //SpawnFarmer();
                }
                yield return new WaitForSeconds(2f);
            }
        }
    }
}