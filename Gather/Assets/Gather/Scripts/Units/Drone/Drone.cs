using UnityEngine;

namespace gather
{
    public abstract class Drone : Unit
    {
        protected Queen myQueen;
        protected Transform queensTransform;
        public bool hasTarget;
        public float orbitRadius = 40;

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

        public void MoveRandomly(Vector2 center)
        {
            Vector2 direction = center + Random.insideUnitCircle * orbitRadius;
            SetDestination(direction);
        }

        public virtual Vector2 AnchorPoint()
        {
            return myQueen.Location();
        }

        public virtual void ReturnToQueen()
        {
            SetDestination(queensTransform.position);
        }
    }
}