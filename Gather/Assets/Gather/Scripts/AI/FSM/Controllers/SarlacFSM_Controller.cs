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
            DroneState_Return returnHome = new DroneState_Return(bb);
            DroneState_Hunt huntState = new DroneState_Hunt(bb);
            DroneState_Engage engageState = new DroneState_Engage(bb);
            DroneState_MoveRandom moveRandomState = new DroneState_MoveRandom(bb);
            initialState = sleepState;

            To_SarlacState_Return toStateReturn = new To_SarlacState_Return(bb, returnHome);
            To_SarlacState_Awake toStateHunt = new To_SarlacState_Awake(bb, huntState);
            To_SarlacState_Sleep toStateSleep = new To_SarlacState_Sleep(bb, sleepState);
            To_SarlacState_Engage toStateEngage = new To_SarlacState_Engage(bb, engageState);
            To_UnitState_MoveRandom toMoveRandom =new To_UnitState_MoveRandom(bb, moveRandomState);


            sleepState.AddTransitions(toStateHunt);
            huntState.AddTransitions(toStateReturn, toStateSleep, toStateEngage, toMoveRandom);
            engageState.AddTransitions(toStateReturn, toStateHunt);
            returnHome.AddTransitions(toStateSleep, toStateHunt);
        }
    }
}