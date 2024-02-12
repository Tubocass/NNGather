using gather;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class State_Return : FSM_State
    {
        Unit unit;
        Transform home;

        public State_Return(Unit unit, Transform home)
        {
            this.unit = unit;
            this.home = home;
        }

        public override void EnterState()
        {

        }

        public override void Update()
        {
            unit.SetDestination(home.position);
        }

        public override void ExitState()
        {

        }
    }
}
