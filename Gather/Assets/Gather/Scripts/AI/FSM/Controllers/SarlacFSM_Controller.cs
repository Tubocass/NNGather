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

            SarlacState_Sleep sleepState = new SarlacState_Sleep(bb);
            SarlacState_Return returnHome = new SarlacState_Return(bb);
            SarlacState_Awake awakeState = new SarlacState_Awake(bb);
            DroneState_Engage engageState = new DroneState_Engage(bb);
            initialState = sleepState;

            To_SarlacState_Return toStateReturn = new To_SarlacState_Return(bb, returnHome);
            To_SarlacState_Awake toStateAwake = new To_SarlacState_Awake(bb, awakeState);
            To_SarlacState_Sleep toStateSleep = new To_SarlacState_Sleep(bb, sleepState);
            To_SarlacState_Engage toStateEngage = new To_SarlacState_Engage(bb, engageState);

            sleepState.AddTransitions(toStateAwake);
            awakeState.AddTransitions(toStateReturn, toStateSleep, toStateEngage);
            engageState.AddTransitions(toStateReturn, toStateAwake);
            returnHome.AddTransitions(toStateSleep, toStateAwake);
        }
    }
}