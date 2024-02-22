using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.States
{
    public class DroneState_LookForFood : FSM_SuperState
    {
        public DroneState_LookForFood(Blackboard context) : base(context)
        {
        }

        public override void Init()
        {
            DroneState_CheckForKnownFood stateCheckKnownFood = new DroneState_CheckForKnownFood(context);
            DroneState_MoveRandom moveRandom = new DroneState_MoveRandom(context);
            DroneState_MoveToNext moveToNext = new DroneState_MoveToNext(context);
            DroneState_Search searchState = new DroneState_Search(context);
            DroneState_Engage engageState = new DroneState_Engage(context);
            initialState = stateCheckKnownFood;

            To_DroneState_MoveToNext toMoveNext = new To_DroneState_MoveToNext(context, moveToNext);
            To_DroneState_MoveRandom toMoveRandom = new To_DroneState_MoveRandom(context, moveRandom);
            To_DroneState_CheckForKnownFood toCheckKnownFood = new To_DroneState_CheckForKnownFood(context, stateCheckKnownFood);
            To_DroneState_Search toSearch = new(context, searchState);
            To_DroneState_Engage toEngage = new(context, engageState);

            stateCheckKnownFood.AddTransitions(toMoveNext, toMoveRandom);
            moveRandom.AddTransitions(toSearch);
            moveToNext.AddTransitions(toSearch);
            searchState.AddTransitions(toEngage, toCheckKnownFood, toMoveRandom, toMoveNext);
            engageState.AddTransitions(toSearch);
        }
    }
}