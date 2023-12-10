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
        //QueenStatus queenStatus;
        SpawnJob job;

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
            job = context.GetValue<SpawnJob>(Configs.SpawnJob);
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
            while (!job.complete)
            {
                if(farmers < job.farmers)
                {
                    farmers++;
                    queen.SpawnFarmer();
                }
                else if(fighters < job.fighters)
                {
                    fighters++;
                    queen.SpawnFighter();
                }else
                {
                    job.complete = true;
                }
                
                yield return new WaitForSeconds(1f);
            }
            Finished?.Invoke();
        }
    }
    public class SpawnJob
    {
        public int farmers;
        public int fighters;
        public bool complete;

        public SpawnJob(int farmers, int fighters)
        {
            this.farmers = farmers;
            this.fighters = fighters;
            complete = false;
        }
    }
}