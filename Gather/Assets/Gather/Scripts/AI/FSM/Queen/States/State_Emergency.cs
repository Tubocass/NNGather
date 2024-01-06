using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Emergency : State_Flee
    {
        Counter fighterCounter;
        FoodCounter foodCounter;
        DroneSpawnConfig spawnConfig;
        Queen queen;

        public State_Emergency(Unit unit, Blackboard context): base(unit, context)
        {
            queen = unit.GetComponent<Queen>();
            spawnConfig = context.GetValue<DroneSpawnConfig>(Configs.SpawnConfig);
            fighterCounter = context.GetValue<TeamConfig>(Configs.TeamConfig)
                .GetUnitCounter(UnitType.Fighter);
            foodCounter = queen.GetFoodCounter();
        }

        public override void EnterState()
        {
            Debug.Log("Emergency!!!");
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
            Debug.Log("Emergency Over");
        }
    }
}