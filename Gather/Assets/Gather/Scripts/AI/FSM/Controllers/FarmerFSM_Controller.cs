using gather;
using Gather.AI.FSM.States;
using Gather.AI.FSM.Transitions;

namespace Gather.AI.FSM.Controllers
{
    public class FarmerFSM_Controller : FSM_Controller
    {
        FarmerDrone drone;
        EnemyDetector enemyDetector;

        protected override void Init()
        {
            drone = GetComponent<FarmerDrone>();
            enemyDetector = GetComponent<EnemyDetector>();
            Blackboard bb = drone.Blackboard;

            DroneState_LookForFood superStateLookForFood = new DroneState_LookForFood(bb);
            DroneState_Flee fleeState = new DroneState_Flee(bb);
            DroneState_Return returnState = new DroneState_Return(bb);
            initialState = superStateLookForFood;

            To_DroneState_LookForFood toLookForFood = new To_DroneState_LookForFood(bb, superStateLookForFood);
            To_DroneState_Flee toFlee = new(bb, fleeState);
            To_DroneState_Return toReturn = new(bb, returnState);

            superStateLookForFood.AddTransitions(toFlee, toReturn);
            fleeState.AddTransitions(toLookForFood, toReturn);
            returnState.AddTransitions(toFlee, toLookForFood);
        }

        public override void Tick()
        {
            enemyDetector.Detect();
            base.Tick();
        }
    }
}