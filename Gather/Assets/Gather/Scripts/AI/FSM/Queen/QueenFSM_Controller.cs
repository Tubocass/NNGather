using gather;

namespace Gather.AI
{
    public class QueenFSM_Controller : FSM_Controller
    {
        State_Move moveState;
        State_Feed feedState;
        State_Spawn spawnState;
        State_Emergency emergencyState;
        Queen queen;

        protected override void Init()
        {
            queen = GetComponent<Queen>();
            Blackboard context = queen.GetBlackboard();

            moveState = new State_Move(queen, context);
            feedState = new State_Feed(queen, context);
            spawnState = new State_Spawn(queen, context);
            emergencyState = new State_Emergency(queen, context);

            initialState = spawnState;

            moveState.transistions.Add(new ToStateFeed(queen, feedState));
            moveState.transistions.Add(new ToStateSpawn(queen, spawnState));
            moveState.transistions.Add(new ToStateFlee(queen, emergencyState));

            feedState.transistions.Add(new ToStateSpawn(queen, spawnState));
            feedState.transistions.Add(new ToStateMove(queen, moveState));
            feedState.transistions.Add(new ToStateFlee(queen, emergencyState));

            spawnState.transistions.Add(new ToStateFeed(queen, feedState));
            spawnState.transistions.Add(new ToStateMove(queen, moveState));
            spawnState.transistions.Add(new ToStateFlee(queen, emergencyState));

            emergencyState.transistions.Add(new ToStateFeed(queen, feedState));
            emergencyState.transistions.Add(new ToStateSpawn(queen, spawnState));
        }

        public override void Tick()
        {
            queen.DetectEnemeies();
            base.Tick();
        }
    }
}
