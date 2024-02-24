using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.States
{
    public class SarlacStateFactory : UnitStateFactory
    {
        public SarlacStateFactory(Blackboard context) : base(context)
        {
        }
        
        SarlacState_Sleep sarlacState_sleep;

        SarlacTransitionTo_Sleep to_Sleep;
        SarlacTransitionTo_Engage to_Engage;
        SarlacTransitionTo_Hunt to_Hunt;
        SarlacTransitionTo_Return to_Return;

        public SarlacState_Sleep SarlacState_Sleep => sarlacState_sleep ??= new SarlacState_Sleep(context);

        public SarlacTransitionTo_Sleep To_Sleep => to_Sleep ??= new SarlacTransitionTo_Sleep(context, sarlacState_sleep);
        public SarlacTransitionTo_Engage To_Engage => to_Engage ??= new SarlacTransitionTo_Engage(context, UnitState_Engage);
        public new SarlacTransitionTo_Hunt To_Hunt => to_Hunt ??= new SarlacTransitionTo_Hunt(context, UnitState_Hunt);
        public SarlacTransitionTo_Return To_Return => to_Return ??= new SarlacTransitionTo_Return(context, UnitState_Engage);
    }
}