using gather;
using Gather.AI.FSM.States;
using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.Controllers
{
    public class FarmerFSM_Controller : FSM_Controller
    {
        FarmerDrone drone;
        EnemyDetector enemyDetector;

        protected override void Init()
        {
            drone = GetComponent<FarmerDrone>();
            enemyDetector = GetComponent<EnemyDetector>();
            Blackboard bb = drone.Blackboard;

            /*
             * initial = CheckKnownSources
             * if none are known, move randomly
             * else move to nearest
             * Scan After Move
             *  if food, engage
             *  else continue
            */
            DroneState_LookForFood superStateLookForFood = new DroneState_LookForFood(bb);
            DroneState_CheckForKnownFood stateCheckKnownFood = new DroneState_CheckForKnownFood(bb);
            DroneState_MoveRandom moveRandom = new DroneState_MoveRandom(bb);
            DroneState_MoveToNext moveToNext = new DroneState_MoveToNext(bb);
            DroneState_Search searchState = new DroneState_Search(bb);
            DroneState_Engage engageState = new DroneState_Engage(bb);
            DroneState_Flee fleeState = new DroneState_Flee(bb);
            DroneState_Return returnState = new DroneState_Return(bb);
            initialState = searchState;

            To_DroneState_MoveToNext toMoveNext = new To_DroneState_MoveToNext(bb, moveToNext);
            To_DroneState_MoveRandom toMoveRandom = new To_DroneState_MoveRandom(bb, moveRandom);
            To_DroneState_CheckForKnownFood toCheckKnownFood = new To_DroneState_CheckForKnownFood(bb, stateCheckKnownFood);
            To_DroneState_Search toSearch = new(bb, searchState);
            To_DroneState_Engage toEngage = new(bb, engageState);
            To_DroneState_Flee toFlee = new(bb, fleeState);
            To_DroneState_Return toReturn = new(bb, returnState);

            stateCheckKnownFood.AddTransitions(toMoveNext, toMoveRandom, toFlee);
            moveRandom.AddTransitions(toFlee, toSearch, toReturn);
            moveToNext.AddTransitions(toFlee, toSearch, toReturn);
            searchState.AddTransitions(toFlee, toEngage, toReturn, toCheckKnownFood, toMoveRandom, toMoveNext);
            engageState.AddTransitions(toFlee, toReturn, toSearch);
            fleeState.AddTransitions(toSearch, toReturn);
            returnState.AddTransitions(toFlee, toCheckKnownFood, toMoveRandom);
        }

        public override void Tick()
        {
            enemyDetector.Detect();
            base.Tick();
        }
    }
}