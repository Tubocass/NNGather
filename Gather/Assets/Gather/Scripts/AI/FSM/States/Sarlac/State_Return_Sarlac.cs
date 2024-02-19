using gather;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class State_Return_Sarlac : FSM_State
    {
        Sarlac sarlac;

        public State_Return_Sarlac(Sarlac sarlac)
        {
            this.sarlac = sarlac;
        }

        public override void Update()
        {
            sarlac.ReturnToHome();
        }
    }
}
