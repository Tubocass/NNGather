using gather;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class SarlacState_Return : FSM_State
    {
        Sarlac sarlac;

        public SarlacState_Return(Blackboard context) : base(context)
        {
            this.sarlac = context.GetValue<Sarlac>(Configs.Unit);
        }

        public override void Update()
        {
            sarlac.ReturnToHome();
        }
    }
}
