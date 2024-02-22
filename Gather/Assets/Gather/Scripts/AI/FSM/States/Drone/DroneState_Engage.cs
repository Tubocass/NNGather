using gather;
using UnityEngine;

namespace Gather.AI.FSM.States
{
    public class DroneState_Engage : FSM_State
    {
        Unit unit;
        ITargetable target;
        bool changePath;

        public DroneState_Engage(Blackboard context) : base(context) 
        {
            this.unit = context.GetValue<Unit>(Configs.Unit);
        }

        public override void EnterState()
        {
            changePath = true; 
            target = context.GetValue<ITargetable>(Configs.Target);
        }

        public override void Update()
        {
            if (target == null || !target.CanBeTargeted(unit.GetTeamID()))
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
