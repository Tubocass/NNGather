using gather;

namespace Gather.AI.FSM.States
{
    public class SarlacState_Awake : FSM_SuperState
    {
        Sarlac sarlac;

        public SarlacState_Awake(Blackboard context) : base(context)
        {
            this.sarlac = context.GetValue<Sarlac>(Configs.Unit);
        }

       
    }
}
