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

            SarlacState_Sleep sleepState = new SarlacState_Sleep();
            SarlacState_Return returnHome = new SarlacState_Return(sarlac);
            SarlacState_Awake awakeState = new SarlacState_Awake(sarlac);
            DroneState_Engage engageState = new DroneState_Engage(sarlac);
            initialState = sleepState;

            To_SarlacState_Return toStateReturn = new To_SarlacState_Return(sarlac, returnHome);
            To_SarlacState_Awake toStateAwake = new To_SarlacState_Awake(sarlac, awakeState);
            To_SarlacState_Sleep toStateSleep = new To_SarlacState_Sleep(sarlac, sleepState);
            To_SarlacState_Engage toStateEngage = new To_SarlacState_Engage(sarlac, engageState);

            sleepState.AddTransitions(toStateAwake);
            awakeState.AddTransitions(toStateReturn, toStateSleep, toStateEngage);
            engageState.AddTransitions(toStateReturn, toStateAwake);
            returnHome.AddTransitions(toStateSleep, toStateAwake);
        }
    }
}