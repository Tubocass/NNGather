namespace Gather.AI.FSM.States
{
    public class FarmerState_LookForFood : FSM_SuperState
    {
        public FarmerState_LookForFood(Blackboard context) : base(context)
        {
        }

        public override void Init()
        {
            FarmerStateFactory factory = new FarmerStateFactory(context);
            factory.DroneState_CheckKnownFood.AddTransitions(factory.ToMoveNext, factory.ToFarmerMoveRandom);
            factory.DroneState_MoveToNext.AddTransitions(factory.ToSearch);
            factory.UnitState_MoveRandom.AddTransitions(factory.ToSearch);
            factory.DroneState_Search.AddTransitions(factory.ToEngage, factory.ToCheckKnownFood, factory.ToMoveNext, factory.ToFarmerMoveRandom);
            factory.UnitState_Engage.AddTransitions(factory.ToSearch);

            initialState = factory.DroneState_Search;
        }
    }
}