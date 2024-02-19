using UnityEngine;
using gather;

namespace Gather.AI.FSM.States
{
    public class State_Emergency : State_Flee
    {
        Counter fighterCounter;
        FoodManager foodCounter;
        DroneSpawnConfig spawnConfig;
        Queen queen;

        public State_Emergency(Queen queen): base(queen)
        {
            this.queen = queen;
            spawnConfig = queen.Blackboard.GetValue<DroneSpawnConfig>(Configs.SpawnConfig);
            fighterCounter = queen.Blackboard.GetValue<TeamConfig>(Configs.TeamConfig)
                .GetUnitCounter(UnitType.Fighter);
            foodCounter = queen.GetComponent<FoodManager>();
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