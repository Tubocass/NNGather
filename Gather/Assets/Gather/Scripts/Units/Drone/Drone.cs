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
        //protected State_Return returnState;
        protected AIController_Interface AIController;

        public IBehaviorState BehaviorState
        {
            get { return behaviorState; }
            set
            {
                if (behaviorState != null)
                {
                    behaviorState.ExitState();
                }
                behaviorState = value;
                behaviorState.EnterState();
            }
        }
        private IBehaviorState behaviorState;

        protected override void Awake()
        {
            base.Awake();

            //returnState = new State_Return(this);
            navAgent.OnDestinationReached += ReachedDestination;

        }

        protected virtual void OnDisable()
        {
            //base.OnDisable();
            //myQueen = null;
            queensTransform = null;
            AIController?.Disable();
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

        //protected virtual void QueenMoved()
        //{
        //}
        public void SetDestination(Vector2 location)
        {
            navAgent.SetDestination(location);
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
