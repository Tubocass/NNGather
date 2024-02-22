using UnityEngine;
using gather;

namespace Gather.AI.FSM.States
{
    public class Queen_State_Emergency : DroneState_Flee
    {
        Counter fighterCounter;
        QueenFoodManager foodCounter;
        DroneSpawnConfig spawnConfig;
        Queen queen;

        public Queen_State_Emergency(Blackboard context) : base(context)
        {
            this.queen = context.GetValue<Queen>(Configs.Unit);
            spawnConfig = context.GetValue<DroneSpawnConfig>(Configs.SpawnConfig);
            fighterCounter = queen.TeamConfig.UnitManager.GetUnitCounter(UnitType.Fighter);
            foodCounter = queen.GetComponent<QueenFoodManager>();
        }

        public override void EnterState()
        {
            //Debug.Log("Emergency!!!");
        }

        public override void Update()
        {
            if(!foodCounter.IsFoodLow() && fighterCounter.Amount < spawnConfig.fighterCap)
            {
                queen.SpawnFighter();
            }
            base.Update();
        }

        public override void ExitState()
        {
            //Debug.Log("Emergency Over");
        }
    }
}