
using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.States
{
    public class QueenStateFactory : UnitStateFactory
    {
        public QueenStateFactory(Blackboard context) : base(context)
        {
        }

        QueenState_Move queenState_Move;
        QueenState_Feed queenState_Feed;
        QueenState_Spawn queenState_Spawn;
        QueenState_Emergency queen_State_Emergency;

        QueenTransitionTo_Feed toFeed;
        QueenTransitionTo_Spawn toSpawn;
        QueenTransitionTo_Move toMove;
        UnitTransitionTo_Flee toFlee;

        public QueenState_Move QueenState_Move => queenState_Move ??= new QueenState_Move(context);
        public QueenState_Feed QueenState_Feed => queenState_Feed ??= new QueenState_Feed(context);
        public QueenState_Spawn QueenState_Spawn => queenState_Spawn ??= new QueenState_Spawn(context);
        public QueenState_Emergency Queen_State_Emergency => queen_State_Emergency ??= new QueenState_Emergency(context);
       
        public QueenTransitionTo_Feed ToFeed => toFeed ??= new QueenTransitionTo_Feed(context, QueenState_Feed);
        public QueenTransitionTo_Spawn ToSpawn => toSpawn ??= new QueenTransitionTo_Spawn(context, QueenState_Feed);
        public QueenTransitionTo_Move ToMove => toMove ??= new QueenTransitionTo_Move(context, QueenState_Feed);
        public new UnitTransitionTo_Flee ToFlee => toFlee ??= new UnitTransitionTo_Flee(context, Queen_State_Emergency);
    }
}