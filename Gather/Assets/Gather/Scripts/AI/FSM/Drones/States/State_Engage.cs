using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Engage : FSM_State
    {
        Blackboard context;
        Unit unit;
        ITargetable target;
        bool changePath;

        public State_Engage(Unit drone, Blackboard bb)
        {
            this.unit = drone;
            context = bb;
        }

        public override void EnterState()
        {
            changePath = true; //if moving when entering state
            target = context.GetValue<ITargetable>(Configs.Target);
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
