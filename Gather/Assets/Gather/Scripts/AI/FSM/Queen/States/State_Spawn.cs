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
        SpawnConfig spawnConfig;
        Counter farmerCounter;
        Counter fighterCounter;
        TeamConfig teamConfig;

        public State_Spawn (Queen queen, Blackboard context)
        {
            this.queen = queen;
            this.context = context;
            spawnConfig = context.GetValue<SpawnConfig>(Configs.SpawnConfig);
            teamConfig = context.GetValue<TeamConfig>(Configs.TeamConfig);
            farmerCounter = teamConfig.GetUnitCounter(UnitType.Farmer);
            fighterCounter = teamConfig.GetUnitCounter(UnitType.Fighter);
        }

        public override void EnterState()
        {
            queen.StartCoroutine(SpawnDrones());
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
                if (chance >= 0.66)
                {
                    if(fighterCounter.amount < spawnConfig.fighterCap)
                    {
                        queen.SpawnFighter();
                    }
                }
                else
                {
                    if (farmerCounter.amount < spawnConfig.farmerCap)
                    {
                        queen.SpawnFarmer();
                    }
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
}