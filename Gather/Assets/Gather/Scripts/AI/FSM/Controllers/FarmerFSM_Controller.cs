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

            /*
             * initial = CheckKnownSources
             * if none are known, move randomly
             * else move to nearest
             * Scan After Move
             *  if food, engage
             *  else continue
            */
            DroneState_CheckForKnownFood stateCheckKnownFood = new DroneState_CheckForKnownFood(drone);
            DroneState_MoveRandom moveRandom = new DroneState_MoveRandom(drone);
            DroneState_MoveToNext moveToNext = new DroneState_MoveToNext(drone);
            DroneState_Search searchState = new DroneState_Search(drone);
            DroneState_Engage engageState = new DroneState_Engage(drone);
            DroneState_Flee fleeState = new DroneState_Flee(drone);
            DroneState_Return returnState = new DroneState_Return(drone);
            initialState = searchState;

            To_DroneState_MoveToNext toMoveNext = new To_DroneState_MoveToNext(drone, moveToNext);
            To_DroneState_MoveRandom toMoveRandom = new To_DroneState_MoveRandom(drone, moveRandom);
            To_DroneState_CheckForKnownFood toCheckKnownFood = new To_DroneState_CheckForKnownFood(drone, stateCheckKnownFood);
            To_DroneState_Search toSearch = new(drone, searchState);
            To_DroneState_Engage toEngage = new(drone, engageState);
            To_DroneState_Flee toFlee = new(drone, fleeState);
            To_DroneState_Return toReturn = new(drone, returnState);

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