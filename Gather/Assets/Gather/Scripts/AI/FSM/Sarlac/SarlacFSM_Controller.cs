using gather;

namespace Gather.AI
{
    public class SarlacFSM_Controller : FSM_Controller
    {
        Sarlac sarlac;

        protected override void Init()
        {
            sarlac = GetComponent<Sarlac>();
            Blackboard bb = sarlac.GetBlackboard();

            State_Sleep sleepState = new State_Sleep();
            State_Awake awakeState = new State_Awake();

            ToStateAwake toStateAwake = new ToStateAwake(sarlac, awakeState);
            ToStateSleep toStateSleep = new ToStateSleep(sarlac, sleepState);

            sleepState.AddTransitions(toStateAwake);
            awakeState.AddTransitions(toStateSleep);
        }
    }
}