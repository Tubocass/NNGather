using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;
using Gather.AI;

namespace gather
{
    public abstract class Drone : Unit
    {
        protected Queen myQueen;
        protected Transform queensTransform;
        protected AIController_Interface AIController;

        protected override void Awake()
        {
            base.Awake();

            navAgent.OnDestinationReached += ReachedDestination;
        }

        protected virtual void OnDisable()
        {
            AIController?.Disable();
            queensTransform = null;
        }

        public virtual void SetQueen(Queen queenie)
        {
            this.myQueen = queenie;
            queensTransform = queenie.transform;
        }

        public Queen GetMyQueen()
        {
            return myQueen;
        }

        protected virtual void ReachedDestination()
        {
            AIController?.AssessSituation();
        }

        public void HaltNavigation()
        {
            navAgent.Stop();
        }

        public void MoveRandomly()
        {
            Vector2 direction = Random.insideUnitCircle * 20;
            SetDestination(direction);
        }

        public virtual void ReturnToQueen()
        {
            SetDestination(queensTransform.position);
        }
    }
}
