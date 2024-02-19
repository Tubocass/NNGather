using gather;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class SarlacState_Return : FSM_State
    {
        Sarlac sarlac;

        public SarlacState_Return(Sarlac sarlac)
        {
            this.sarlac = sarlac;
        }

        public override void Update()
        {
            sarlac.ReturnToHome();
        }
    }
}
