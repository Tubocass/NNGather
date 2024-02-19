using gather;

namespace Gather.AI.FSM.States
{
    public class DroneState_Engage : FSM_State
    {
        Unit unit;
        ITargetable target;
        bool changePath;

        public DroneState_Engage(Unit drone)
        {
            this.unit = drone;
        }

        public override void EnterState()
        {
            changePath = true; //if moving when entering state
            target = unit.Blackboard.GetValue<ITargetable>(Configs.Target);
        }

        public override void Update()
        {
            if (target == null || !target.CanTarget(unit.GetTeam()))
            {
                unit.SetHasTarget(false);
                return;
            }
            
            if(!unit.IsMoving || changePath)
            {
                unit.SetDestination(target.GetLocation());
            }
        }

        public override void ExitState()
        {
            target = null;
            unit.SetHasTarget(false);
        }
    }
}
