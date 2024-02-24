using gather;

namespace Gather.AI.FSM.States
{
    public class UnitState_MoveRandom: FSM_State
    {
        IRoamer roamer;

        public UnitState_MoveRandom(Blackboard context) : base(context)
        {
            this.roamer = context.GetValue<IRoamer>(Configs.Unit);
        }

        public override void EnterState()
        {
            roamer.MoveRandomly(roamer.AnchorPoint());
        }

    }
}