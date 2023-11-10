using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

namespace gather
{
    public abstract class Drone : Unit
    {
        protected Queen myQueen;
        protected Transform queensTransform;
        protected State_Return returnState;

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
            
            returnState = new State_Return(this);
            navAgent.OnDestinationReached += ReachedDestination;

        }

        protected virtual void OnDisable()
        {
            //base.OnDisable();
            //myQueen = null;
            queensTransform = null;
            if (BehaviorState != null)
            {
                BehaviorState.ExitState();
            }
        }

        public virtual void SetQueen(Queen queenie)
        {
            this.myQueen = queenie;
            queensTransform = queenie.transform;
        }

        protected virtual void QueenMoved()
        {

        }
        public void SetDestination(Vector2 location)
        {
            navAgent.SetDestination(location);
        }

        protected virtual void ReachedDestination()
        {
            if (BehaviorState != null)
            {
                BehaviorState.AssesSituation();
            }
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
