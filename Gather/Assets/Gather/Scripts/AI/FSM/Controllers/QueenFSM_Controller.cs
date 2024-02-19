using gather;
using Gather.AI.FSM.States;
using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.Controllers
{
    public class QueenFSM_Controller : FSM_Controller
    {
        Queen queen;
        EnemyDetector enemyDetector;

        protected override void Init()
        {
            queen = GetComponent<Queen>();
            enemyDetector = GetComponent<EnemyDetector>();

            QueenState_Move moveState = new QueenState_Move(queen);
            QueenState_Feed feedState = new QueenState_Feed(queen);
            QueenState_Spawn spawnState = new QueenState_Spawn(queen);
            Queen_State_Emergency emergencyState = new Queen_State_Emergency(queen);
            initialState = spawnState;

            To_QueenState_Feed toFeed = new To_QueenState_Feed(queen, feedState);
            To_QueenState_Spawn toSpawn = new To_QueenState_Spawn(queen, spawnState);
            To_DroneState_Flee toFlee = new To_DroneState_Flee(queen, emergencyState);
            To_QueenState_Move toMove = new To_QueenState_Move(queen, moveState);

            moveState.AddTransitions(toFlee, toFeed, toSpawn);
            feedState.AddTransitions(toFlee, toMove, toSpawn);
            spawnState.AddTransitions(toFlee, toFeed, toMove);
            emergencyState.AddTransitions(toFeed, toSpawn);
        }

        public override void Tick()
        {
            enemyDetector.Detect();
            base.Tick();
        }
    }
}
