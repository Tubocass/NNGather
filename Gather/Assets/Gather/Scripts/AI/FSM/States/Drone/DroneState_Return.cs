using gather;

namespace Gather.AI.FSM.States
{
    public class DroneState_Return : FSM_State
    {
        IRoamer roamer;

        public DroneState_Return(Blackboard context) : base(context)
        {
            this.roamer = context.GetValue<IRoamer>(Configs.Unit);
        }

        public override void Update()
        {
            roamer.ReturnHome();   
        }
    }
}
