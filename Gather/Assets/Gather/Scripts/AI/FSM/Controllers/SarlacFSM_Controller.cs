using Assets.Gather.Scripts.AI.FSM.Transitions;
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
            State_Return_Sarlac returnHome = new State_Return_Sarlac(sarlac, sarlac.GetHome());
            State_Awake awakeState = new State_Awake(sarlac, bb);
            State_Engage engageState = new State_Engage(sarlac, bb);
            initialState = sleepState;

            ToStateReturnHome toStateReturn = new ToStateReturnHome(sarlac, returnHome);
            ToStateAwake toStateAwake = new ToStateAwake(sarlac, awakeState);
            ToStateSleep toStateSleep = new ToStateSleep(sarlac, sleepState);
            ToStateEngage_Sarlac toStateEngage = new ToStateEngage_Sarlac(sarlac, engageState);

            sleepState.AddTransitions(toStateAwake);
            awakeState.AddTransitions(toStateReturn, toStateSleep, toStateEngage);
            engageState.AddTransitions(toStateReturn, toStateAwake);
            returnHome.AddTransitions(toStateSleep, toStateAwake);
        }
    }
}