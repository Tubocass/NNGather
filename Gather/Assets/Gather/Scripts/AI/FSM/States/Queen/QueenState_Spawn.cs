using UnityEngine;
using Gather;
using System.Collections;

namespace Gather.AI.FSM.States
{
    public class QueenState_Spawn : FSM_State
    {
        Queen queen;
        DroneSpawnConfig spawnConfig;
        Counter farmerCounter;
        Counter fighterCounter;
        float refractoryTime = 1f;

        public QueenState_Spawn(Blackboard context) : base(context)
        {
            this.queen = context.GetValue<Queen>(Configs.Unit);
            spawnConfig = context.GetValue<DroneSpawnConfig>(Configs.SpawnConfig);
            farmerCounter = queen.TeamConfig.UnitManager.GetUnitCounter(UnitType.Farmer);
            fighterCounter = queen.TeamConfig.UnitManager.GetUnitCounter(UnitType.Fighter);
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
                    if (fighterCounter.GetAmount() < spawnConfig.fighterCap)
                    {
                        queen.SpawnFighter();
                    }
                } else
                {
                    if (farmerCounter.GetAmount() < spawnConfig.farmerCap)
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