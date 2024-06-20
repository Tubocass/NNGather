using Gather;

namespace Gather.AI.FSM.States
{
    public class QueenState_Emergency : UnitState_Flee
    {
        Counter fighterCounter;
        QueenFoodManager foodCounter;
        DroneSpawnConfig spawnConfig;
        Queen queen;

        public QueenState_Emergency(Blackboard context) : base(context)
        {
            this.queen = context.GetValue<Queen>(Keys.Unit);
            spawnConfig = context.GetValue<DroneSpawnConfig>(Keys.SpawnConfig);
            fighterCounter = queen.TeamConfig.UnitManager.GetUnitCounter(UnitType.Fighter);
            foodCounter = queen.GetComponent<QueenFoodManager>();
        }

        public override void EnterState()
        {
            //Debug.Log("Emergency!!!");
        }

        public override void Update()
        {
            if(!foodCounter.IsFoodLow() && fighterCounter.GetAmount() < spawnConfig.fighterCap)
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