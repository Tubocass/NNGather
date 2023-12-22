using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Engage : FSM_State
    {
        Blackboard context;
        Drone drone;
        ITarget target;
        bool changePath;

        public State_Engage(Drone drone, Blackboard bb)
        {
            this.drone = drone;
            context = bb;
        }

        public override void EnterState()
        {
            changePath = true;
            target = context.GetValue<ITarget>(Configs.Target);
        }

        public override void Update()
        {
            if (target == null || !target.CanTarget(drone.GetTeam()))
            {
                drone.hasTarget = false;
            }
            else if(!drone.IsMoving || changePath)
            {
                drone.SetDestination(target.Location());
            }
        }

        public override void ExitState()
        {
            target = null;
            drone.hasTarget = false;
        }

        public override string GetStateName()
        {
            return States.engage;
        }
    }
}
