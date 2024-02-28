using Gather;
using Gather.AI.FSM.States;

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
            QueenStateFactory factory = new QueenStateFactory(bb);

            factory.QueenState_Feed.AddTransitions(factory.ToFlee, factory.ToMove, factory.ToSpawn);
            factory.QueenState_Move.AddTransitions(factory.ToFlee, factory.ToFeed, factory.ToSpawn);
            factory.QueenState_Spawn.AddTransitions(factory.ToFlee, factory.ToFeed, factory.ToMove);
            factory.Queen_State_Emergency.AddTransitions(factory.ToFeed, factory.ToSpawn);

            initialState = factory.QueenState_Spawn;
        }

        public override void Tick()
        {
            enemyDetector.Detect();
            base.Tick();
        }
    }
}
