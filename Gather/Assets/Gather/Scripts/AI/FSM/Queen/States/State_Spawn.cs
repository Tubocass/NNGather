using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Spawn : FSM_State
    {
        Queen queen;
        SpawnConfig spawnConfig;
        Counter farmerCounter;
        Counter fighterCounter;
        TeamConfig teamConfig;

        public State_Spawn (Queen queen, Blackboard context)
        {
            this.queen = queen;
            spawnConfig = context.GetValue<SpawnConfig>(Configs.SpawnConfig);
            teamConfig = context.GetValue<TeamConfig>(Configs.TeamConfig);
            farmerCounter = teamConfig.GetUnitCounter(UnitType.Farmer);
            fighterCounter = teamConfig.GetUnitCounter(UnitType.Fighter);
        }

        public override void EnterState()
        {
            
        }

        public override void Update()
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
        }

        public override void ExitState()
        {
        }
    }
}