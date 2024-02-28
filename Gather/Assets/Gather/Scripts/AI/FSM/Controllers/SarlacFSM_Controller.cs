using Gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Controllers
{
    public class SarlacFSM_Controller : FSM_Controller
    {
        Sarlac sarlac;
        EnemyDetector enemyDetector;

        protected override void Init()
        {
            sarlac = GetComponent<Sarlac>();
            enemyDetector = GetComponent<EnemyDetector>();
            Blackboard bb = sarlac.Blackboard;
            SarlacStateFactory factory = new SarlacStateFactory(bb);

            factory.SarlacState_Sleep.AddTransitions(factory.To_Awake);
            factory.SarlacState_Awake.AddTransitions(factory.To_Return);
            factory.UnitState_Return.AddTransitions(factory.To_Sleep, factory.To_Awake);

            initialState = factory.SarlacState_Sleep;
        }

        public override void Tick()
        {
            enemyDetector.Detect();
            base.Tick();
        }
    }
}