using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_UnitState_MoveRandom : FSM_Transition
    {

        public To_UnitState_MoveRandom(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
        }

        public override bool IsValid()
        {
            return !unit.IsMoving && !context.GetValue<bool>(Configs.HasTarget);
        }
    }
}