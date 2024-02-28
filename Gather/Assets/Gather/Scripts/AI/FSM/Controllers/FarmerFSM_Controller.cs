using Gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Controllers
{
    public class FarmerFSM_Controller : FSM_Controller
    {
        FarmerDrone drone;
        EnemyDetector enemyDetector;
        FarmerStateFactory factory;

        protected override void Init()
        {
            drone = GetComponent<FarmerDrone>();
            enemyDetector = GetComponent<EnemyDetector>();
            Blackboard bb = drone.Blackboard;
            factory = new FarmerStateFactory(bb);

            factory.DroneState_LookForFood.AddTransitions(factory.ToFlee, factory.ToReturn);
            factory.UnitState_Flee.AddTransitions(factory.ToLookForFood, factory.ToReturn);
            factory.UnitState_Return.AddTransitions(factory.ToFlee, factory.ToLookForFood);
            
            initialState = factory.DroneState_LookForFood;
        }

        public override void Tick()
        {
            enemyDetector.Detect();
            base.Tick();
        }
    }
}