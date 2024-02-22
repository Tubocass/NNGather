using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.States
{
    public class DroneStateFactory
    {
        Blackboard context;

        public DroneStateFactory(Blackboard context)
        {
            this.context = context;
        }

        DroneState_LookForFood droneState_LookForFood;
        DroneState_CheckForKnownFood droneState_CheckKnownFood;
        DroneState_MoveToNext droneState_MoveToNext;
        DroneState_MoveRandom droneState_MoveRandom;
        DroneState_Search droneState_Search;
        DroneState_Engage droneState_Engage;
        DroneState_Return droneState_Return;
        DroneState_Flee droneState_Flee;
        DroneState_Hunt droneState_Hunt;

        To_DroneState_LookForFood toLookForFood;
        To_DroneState_CheckForKnownFood toCheckKnownFood;
        To_DroneState_MoveToNext toMoveNext;
        To_DroneState_MoveRandom toMoveRandom;
        To_DroneState_Search toSearch;
        To_DroneState_Engage toEngage;
        To_DroneState_Flee toFlee;
        To_DroneState_Return toReturn;
        To_DroneState_Hunt toHunt;

        public DroneState_LookForFood DroneState_LookForFood => droneState_LookForFood ??= new DroneState_LookForFood(context);
        public DroneState_Return DroneState_Return => droneState_Return ??= new DroneState_Return(context);
        public DroneState_Flee DroneState_Flee => droneState_Flee ??= new DroneState_Flee(context);
        public DroneState_CheckForKnownFood DroneState_CheckKnownFood => droneState_CheckKnownFood ??= new DroneState_CheckForKnownFood(context);
        public DroneState_MoveToNext DroneState_MoveToNext => droneState_MoveToNext ??= new DroneState_MoveToNext(context);
        public DroneState_MoveRandom DroneState_MoveRandom => droneState_MoveRandom ??= new DroneState_MoveRandom(context);
        public DroneState_Search DroneState_Search => droneState_Search ??= new DroneState_Search(context);
        public DroneState_Engage DroneState_Engage => droneState_Engage ??= new DroneState_Engage(context);
        public DroneState_Hunt DroneState_Hunt => droneState_Hunt ??= new DroneState_Hunt(context);

        public To_DroneState_LookForFood ToLookForFood => toLookForFood ??= new To_DroneState_LookForFood(context, DroneState_LookForFood);
        public To_DroneState_CheckForKnownFood ToCheckKnownFood => toCheckKnownFood ??= new To_DroneState_CheckForKnownFood(context, DroneState_CheckKnownFood);
        public To_DroneState_MoveToNext ToMoveNext => toMoveNext ??= new To_DroneState_MoveToNext(context, DroneState_MoveToNext);
        public To_DroneState_MoveRandom ToMoveRandom => toMoveRandom ??= new To_DroneState_MoveRandom(context, DroneState_MoveRandom);
        public To_DroneState_Search ToSearch => toSearch ??= new To_DroneState_Search(context, DroneState_Search);
        public To_DroneState_Engage ToEngage => toEngage ??= new To_DroneState_Engage(context, DroneState_Engage);
        public To_DroneState_Flee ToFlee => toFlee ??= new To_DroneState_Flee(context, DroneState_Flee);
        public To_DroneState_Return ToReturn => toReturn ??= new To_DroneState_Return(context, DroneState_Return);
        public To_DroneState_Hunt To_Hunt => toHunt ??= new To_DroneState_Hunt(context, DroneState_Hunt);

    }
}