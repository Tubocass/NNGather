namespace Gather.AI.FSM.States
{
    public class DroneState_LookForFood : FSM_SuperState
    {
        public DroneState_LookForFood(Blackboard context) : base(context)
        {
        }

        public override void Init()
        {
            DroneStateFactory factory = new DroneStateFactory(context);
            factory.DroneState_CheckKnownFood.AddTransitions(factory.ToMoveNext, factory.ToFarmerMoveRandom);
            factory.DroneState_MoveToNext.AddTransitions(factory.ToSearch);
            factory.DroneState_MoveRandom.AddTransitions(factory.ToSearch);
            factory.DroneState_Search.AddTransitions(factory.ToEngage, factory.ToCheckKnownFood, factory.ToMoveNext, factory.ToFarmerMoveRandom);
            factory.DroneState_Engage.AddTransitions(factory.ToSearch);

            initialState = factory.DroneState_Search;
        }
    }
}