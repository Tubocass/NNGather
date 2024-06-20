using Gather;

namespace Gather.AI.FSM.States
{
    public class SarlacState_Awake : FSM_SuperState
    {
        Sarlac sarlac;
        public SarlacState_Awake(Blackboard context) : base(context)
        {
            sarlac = context.GetValue<Sarlac>(Keys.Unit);
        }

        public override void EnterState()
        {
            base.EnterState();
            sarlac.WakeUp();
        }

        public override void Init()
        {
            SarlacStateFactory factory = new SarlacStateFactory(context);

            factory.UnitState_Hunt.AddTransitions(
                factory.To_Engage, factory.To_Return, factory.ToMoveRandom, factory.To_Sleep
                );
            factory.UnitState_MoveRandom.AddTransitions(factory.To_Hunt, factory.To_Return);
            factory.UnitState_Engage.AddTransitions(factory.To_Hunt, factory.To_Hunt, factory.To_Return);
            
            initialState = factory.UnitState_Hunt;
        }
    }
}
