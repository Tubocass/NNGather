using gather;

namespace Gather.AI
{
    public class QueenFSM_Controller : FSM_Controller
    {
        State_Move moveState;
        State_Feed feedState;
        State_Spawn spawnState;
        Queen queen;

        protected override void Init()
        {
            queen = GetComponent<Queen>();
            Blackboard context = queen.GetBlackboard();

            moveState = new State_Move(queen, context);
            feedState = new State_Feed(queen, context);
            spawnState = new State_Spawn(queen, context);
            initialState = spawnState;

            moveState.transistions.Add(new ToStateFeed(queen, feedState));
            moveState.transistions.Add(new ToStateSpawn(queen, spawnState));

            feedState.transistions.Add(new ToStateSpawn(queen, spawnState));
            feedState.transistions.Add(new ToStateMove(queen, moveState));

            spawnState.transistions.Add(new ToStateFeed(queen, feedState));
            spawnState.transistions.Add(new ToStateMove(queen, moveState));
        }

        public override void Tick()
        {
            queen.DetectEnemeies();
            base.Tick();
        }
    }
}
