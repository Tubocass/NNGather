using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.States
{
    public class UnitStateFactory
    {
        protected Blackboard context;

        public UnitStateFactory(Blackboard context)
        {
            this.context = context;
        }

       
        UnitState_MoveRandom unitState_MoveRandom;
        UnitState_Engage unitState_Engage;
        UnitState_Return unitState_Return;
        UnitState_Flee unitState_Flee;
        UnitState_Hunt unitState_Hunt;
        
        UnitTransitionTo_MoveRandom toMoveRandom;
        UnitTransitionTo_Engage toEngage;
        UnitTransitionTo_Flee toFlee;
        UnitTransitionTo_Hunt toHunt;

        public UnitState_Return UnitState_Return => unitState_Return ??= new UnitState_Return(context);
        public UnitState_Flee UnitState_Flee => unitState_Flee ??= new UnitState_Flee(context);
        public UnitState_MoveRandom UnitState_MoveRandom => unitState_MoveRandom ??= new UnitState_MoveRandom(context);
        public UnitState_Engage UnitState_Engage => unitState_Engage ??= new UnitState_Engage(context);
        public UnitState_Hunt UnitState_Hunt => unitState_Hunt ??= new UnitState_Hunt(context);

        public UnitTransitionTo_MoveRandom ToMoveRandom => toMoveRandom ??= new UnitTransitionTo_MoveRandom(context, UnitState_MoveRandom);
        public UnitTransitionTo_Engage ToEngage => toEngage ??= new UnitTransitionTo_Engage(context, UnitState_Engage);
        public UnitTransitionTo_Flee ToFlee => toFlee ??= new UnitTransitionTo_Flee(context, UnitState_Flee);
        public UnitTransitionTo_Hunt To_Hunt => toHunt ??= new UnitTransitionTo_Hunt(context, UnitState_Hunt);

    }
}