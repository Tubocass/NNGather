

using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.States
{
    public class FarmerStateFactory : UnitStateFactory
    {
        public FarmerStateFactory(Blackboard context) : base(context)
        {
        }

        FarmerState_LookForFood droneState_LookForFood;
        FarmerState_CheckForKnownFood droneState_CheckKnownFood;
        FarmerState_Search droneState_Search;
        FarmerState_MoveToNext droneState_MoveToNext;

        FarmerTransitionTo_LookForFood toLookForFood;
        FarmerTransitionTo_CheckForKnownFood toCheckKnownFood;
        FarmerTransitionTo_MoveToNext toMoveNext;
        FarmerTransitionTo_MoveRandom toFarmerMoveRandom;
        FarmerTransitionTo_Search toSearch;
        FarmerTransitionTo_Return toReturn;


        public FarmerState_LookForFood DroneState_LookForFood => droneState_LookForFood ??= new FarmerState_LookForFood(context);
        public FarmerState_CheckForKnownFood DroneState_CheckKnownFood => droneState_CheckKnownFood ??= new FarmerState_CheckForKnownFood(context);
        public FarmerState_MoveToNext DroneState_MoveToNext => droneState_MoveToNext ??= new FarmerState_MoveToNext(context);
        public FarmerState_Search DroneState_Search => droneState_Search ??= new FarmerState_Search(context);

        public FarmerTransitionTo_LookForFood ToLookForFood => toLookForFood ??= new FarmerTransitionTo_LookForFood(context, DroneState_LookForFood);
        public FarmerTransitionTo_CheckForKnownFood ToCheckKnownFood => toCheckKnownFood ??= new FarmerTransitionTo_CheckForKnownFood(context, DroneState_CheckKnownFood);
        public FarmerTransitionTo_MoveToNext ToMoveNext => toMoveNext ??= new FarmerTransitionTo_MoveToNext(context, DroneState_MoveToNext);
        public FarmerTransitionTo_MoveRandom ToFarmerMoveRandom => toFarmerMoveRandom ??= new FarmerTransitionTo_MoveRandom(context, UnitState_MoveRandom);
        public FarmerTransitionTo_Search ToSearch => toSearch ??= new FarmerTransitionTo_Search(context, DroneState_Search);
        public FarmerTransitionTo_Return ToReturn => toReturn ??= new FarmerTransitionTo_Return(context, UnitState_Return);
    }
}