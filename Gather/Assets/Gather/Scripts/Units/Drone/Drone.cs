using UnityEngine;

namespace gather
{
    [RequireComponent(typeof(PooledObject))]
    public abstract class Drone : Unit
    {
        protected Queen myQueen;
        [SerializeField] float orbitRadius = 40;
        PooledObject po;

        protected override void Awake()
        {
            base.Awake();
            po = GetComponent<PooledObject>();
        }

        public override void Death()
        {
            myQueen = null;
            po.ReleaseToPool();
            base.Death();
        }

        public virtual void SetQueen(Queen queenie)
        {
            this.myQueen = queenie;
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
            return myQueen.GetLocation();
        }
    }
}