using System.Collections;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Spawn : FSM_State
    {
        public GameEvent Finished;
        Queen queen;
        Blackboard context;
        //TeamConfig teamConfig;
        //private int farmers, fighters;
        //QueenStatus queenStatus;
        SpawnJob job;

        public State_Spawn (Queen queen, Blackboard context)
        {
            this.queen = queen;
            this.context = context;
        }

        public override void EnterState()
        {
            // spawn how many of what
            //farmers = 0;
            //fighters = 0;
            //job = context.GetValue<SpawnJob>(Configs.SpawnJob);
            queen.StartCoroutine(SpawnDrones());
            //Debug.Log("Spawning");
        }

        public override void Update()
        {
            
        }

        public override void ExitState()
        {
            queen.StopCoroutine(SpawnDrones());
        }

        public override string GetStateName()
        {
            return States.spawn;
        }

        IEnumerator SpawnDrones()
        {
            while (true)
            {
                float chance = Random.value;
                if (chance >= 1)
                {
                    queen.SpawnFighter();
                }
                else
                {
                    queen.SpawnFarmer();
                }
                yield return new WaitForSeconds(1f);
            }
            //while (!job.complete)
            //{
            //    if(farmers < job.farmers)
            //    {
            //        farmers++;
            //        queen.SpawnFarmer();
            //    }
            //    else if(fighters < job.fighters)
            //    {
            //        fighters++;
            //        queen.SpawnFighter();
            //    }else
            //    {
            //        job.complete = true;
            //    }

            //    yield return new WaitForSeconds(1f);
            //}
            //Finished?.Invoke();
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