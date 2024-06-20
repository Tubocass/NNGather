using Gather;

namespace Gather.AI.FSM.States
{
    public class UnitState_Return : FSM_State
    {
        IRoamer roamer;

        public UnitState_Return(Blackboard context) : base(context)
        {
            this.roamer = context.GetValue<IRoamer>(Keys.Unit);
        }

        public override void Update()
        {
            roamer.ReturnHome();   
        }
    }
}
