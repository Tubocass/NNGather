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
            Blackboard bb = queen.Blackboard;

            QueenState_Move moveState = new QueenState_Move(bb);
            QueenState_Feed feedState = new QueenState_Feed(bb);
            QueenState_Spawn spawnState = new QueenState_Spawn(bb);
            Queen_State_Emergency emergencyState = new Queen_State_Emergency(bb);
            initialState = spawnState;

            To_QueenState_Feed toFeed = new To_QueenState_Feed(bb, feedState);
            To_QueenState_Spawn toSpawn = new To_QueenState_Spawn(bb, spawnState);
            To_DroneState_Flee toFlee = new To_DroneState_Flee(bb, emergencyState);
            To_QueenState_Move toMove = new To_QueenState_Move(bb, moveState);

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
