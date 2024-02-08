using gather;

namespace Gather.AI
{
    public class ToStateAwake : FSM_Transistion
    {
        private readonly Sarlac sarlac;
        public ToStateAwake(Sarlac sarlac, FSM_State nextState) : base(sarlac, nextState)
        {
            this.sarlac = sarlac;
        }

        public override bool IsValid()
        {
            return sarlac.isNight;
        }

        public override void OnTransition()
        {
        }
    }
}
