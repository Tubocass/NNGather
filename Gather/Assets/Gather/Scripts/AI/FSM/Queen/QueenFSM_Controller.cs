using gather;

namespace Gather.AI
{
    public class QueenFSM_Controller : FSM_Controller
    {
        State_Move moveState;
        State_Feed feedState;
        State_Spawn spawnState;

        public QueenFSM_Controller (Queen queen, Blackboard context)
        {
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
    }
}
