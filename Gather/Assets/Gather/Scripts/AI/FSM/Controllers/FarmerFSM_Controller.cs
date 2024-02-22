using gather;
using Gather.AI.FSM.States;

namespace Gather.AI.FSM.Controllers
{
    public class FarmerFSM_Controller : FSM_Controller
    {
        FarmerDrone drone;
        EnemyDetector enemyDetector;
        DroneStateFactory factory;

        protected override void Init()
        {
            drone = GetComponent<FarmerDrone>();
            enemyDetector = GetComponent<EnemyDetector>();
            Blackboard bb = drone.Blackboard;
            factory = new DroneStateFactory(bb);

            factory.DroneState_LookForFood.AddTransitions(factory.ToFlee, factory.ToReturn);
            factory.DroneState_Flee.AddTransitions(factory.ToLookForFood, factory.ToReturn);
            factory.DroneState_Return.AddTransitions(factory.ToLookForFood);
            
            initialState = factory.DroneState_LookForFood;
        }

        public override void Tick()
        {
            enemyDetector.Detect();
            base.Tick();
        }
    }
}