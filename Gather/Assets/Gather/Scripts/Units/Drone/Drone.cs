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


        protected override void Awake()
        {
            base.Awake();

        }

        protected virtual void OnDisable()
        {
            fsmController?.Disable();
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

        public void HaltNavigation()
        {
            navAgent.Stop();
            isMoving = false;
        }

        public void MoveRandomly()
        {
            Vector2 direction = Location() + Random.insideUnitCircle * 20;
            SetDestination(direction);
        }

        public virtual void ReturnToQueen()
        {
            SetDestination(queensTransform.position);
        }
    }
}
