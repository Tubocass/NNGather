using UnityEngine;

namespace gather
{
    public abstract class Drone : Unit
    {
        protected Queen myQueen;
        protected Transform queensTransform;
        public bool hasTarget;

        //protected override void OnDisable()
        //{
        //    base.OnDisable();
        //}

        public override void Death()
        {
            myQueen = null;
            queensTransform = null;
            base.Death();
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
            Vector2 direction = Location() + Random.insideUnitCircle * 40;
            SetDestination(direction);
        }

        public virtual void ReturnToQueen()
        {
            SetDestination(queensTransform.position);
        }
    }
}
