using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Transitions
{
    public class To_SarlacState_Engage : To_DroneState_Engage
    {
        Sarlac sarlac;
        public To_SarlacState_Engage(Blackboard context, FSM_State nextState) : base(context, nextState)
        {
            sarlac = unit.GetComponent<Sarlac>();
        }

        public override bool IsValid()
        {
            return context.GetValue<bool>(Configs.HasTarget) && sarlac.isNight;
        }
    }
}