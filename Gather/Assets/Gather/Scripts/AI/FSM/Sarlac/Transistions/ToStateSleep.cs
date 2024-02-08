using gather;

namespace Gather.AI
{

    public class ToStateSleep : FSM_Transistion
    {
        private readonly Sarlac sarlac;
        public ToStateSleep(Sarlac sarlac, FSM_State nextState) : base(sarlac, nextState)
        {
            this.sarlac = sarlac;
        }

        public override bool IsValid()
        {
            return !sarlac.isNight && sarlac.IsAtHome();
        }

        public override void OnTransition()
        {
        }
    }
}
