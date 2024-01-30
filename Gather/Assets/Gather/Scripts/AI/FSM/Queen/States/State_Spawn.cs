using UnityEngine;
using gather;
using System.Collections;

namespace Gather.AI
{
    public class State_Spawn : FSM_State
    {
        Queen queen;
        DroneSpawnConfig spawnConfig;
        Counter farmerCounter;
        Counter fighterCounter;
        TeamConfig teamConfig;
        float refractoryTime = 1f;

        public State_Spawn (Queen queen, Blackboard context)
        {
            this.queen = queen;
            spawnConfig = context.GetValue<DroneSpawnConfig>(Configs.SpawnConfig);
            teamConfig = context.GetValue<TeamConfig>(Configs.TeamConfig);
            farmerCounter = teamConfig.GetUnitCounter(UnitType.Farmer);
            fighterCounter = teamConfig.GetUnitCounter(UnitType.Fighter);
        }

        public override void EnterState()
        {
            queen.StartCoroutine(Spawn());
        }

        public override void Update()
        {
          
        }

        IEnumerator Spawn()
        {
            while (queen.isActiveAndEnabled)
            {
                float chance = Random.value;
                if (chance >= 0.66)
                {
                    if (fighterCounter.Amount < spawnConfig.fighterCap)
                    {
                        queen.SpawnFighter();
                    }
                } else
                {
                    if (farmerCounter.Amount < spawnConfig.farmerCap)
                    {
                        queen.SpawnFarmer();
                    }
                }
                yield return new WaitForSeconds(refractoryTime);
            }

        }

        public override void ExitState()
        {
            queen.StopCoroutine(Spawn());
        }
    }
}