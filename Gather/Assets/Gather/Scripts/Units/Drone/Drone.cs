using UnityEngine;

namespace gather
{
    [RequireComponent(typeof(PooledObject))]
    public abstract class Drone : Unit
    {
        protected Queen myQueen;
        protected Transform queensTransform;
        public bool hasTarget;
        public float orbitRadius = 40;
        PooledObject po;

        protected override void Awake()
        {
            base.Awake();
            po = GetComponent<PooledObject>();
        }

        public override void Death()
        {
            myQueen = null;
            queensTransform = null;
            po.ReleaseToPool();
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