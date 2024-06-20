using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class UnitTransitionTo_MoveRandom : FSM_Transition
    {

        public UnitTransitionTo_MoveRandom(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
        }

        public override bool IsValid()
        {
            return !unit.IsMoving && !context.GetValue<bool>(Keys.HasTarget);
        }
    }
}