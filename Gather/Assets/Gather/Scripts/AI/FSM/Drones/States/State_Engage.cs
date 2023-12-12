using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gather;

namespace Gather.AI
{
    public class State_Engage : FSM_State
    {
        Drone droneController;
        ITarget target;
        public event TargetEvent TargetLost;
        public event TargetEvent TargetReached;
        Blackboard context;
        SearchConfig searchConfig;

        public State_Engage(Drone droneController, Blackboard bb)
        {
            this.droneController = droneController;
            context = bb;
            searchConfig = context.GetValue<SearchConfig>(Configs.EnemySearchConfig);
        }

        public void EnterState()
        {
            //Debug.Log("Engaging");
            target = context.GetValue<ITarget>(Configs.Target);
            if(target !=null)
            {
                droneController.SetDestination(target.Location());
                droneController.StartCoroutine(MoveToTarget());

            }
        }

        public void AssesSituation()
        {
            if (!target.CanTarget(droneController.GetTeam())
                || Vector3.Distance(target.Location(), droneController.Location()) > searchConfig.searchDist)
            {
                TargetLost?.Invoke();
            }
            else
            {
                droneController.SetDestination(target.Location());
            }
        }

        public void ExitState()
        {
            target = null;
            droneController.StopAllCoroutines();
        }

        public override string GetStateName()
        {
            return States.engage;
        }

        public IEnumerator MoveToTarget()
        {
            while (target != null && Vector3.Distance(target.Location(), droneController.Location()) > 1)
            {
                droneController.SetDestination(target.Location());
                yield return new WaitForSeconds(0.25f);
            }
            TargetReached?.Invoke();
        }
    }
}
