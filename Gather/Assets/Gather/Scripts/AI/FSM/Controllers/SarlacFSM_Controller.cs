using gather;
using Gather.AI.FSM.States;
using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.Controllers
{
    public class SarlacFSM_Controller : FSM_Controller
    {
        Sarlac sarlac;

        protected override void Init()
        {
            sarlac = GetComponent<Sarlac>();
            Blackboard bb = sarlac.Blackboard;

            State_Sleep sleepState = new State_Sleep();
            State_Return returnHome = new State_Return(sarlac, sarlac.GetHome());
            State_Awake awakeState = new State_Awake(sarlac, bb);
            State_Engage engageState = new State_Engage(sarlac, bb);
            initialState = sleepState;

            ToStateReturnHome toStateReturn = new ToStateReturnHome(sarlac, returnHome);
            ToStateAwake toStateAwake = new ToStateAwake(sarlac, awakeState);
            ToStateSleep toStateSleep = new ToStateSleep(sarlac, sleepState);
            ToStateEngage toStateEngage = new ToStateEngage(sarlac, engageState);

            sleepState.AddTransitions(toStateAwake);
            awakeState.AddTransitions(toStateReturn, toStateSleep, toStateEngage);
        }
    }
}