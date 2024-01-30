using Gather.AI;
using System.Collections;
using UnityEngine;

namespace gather
{
    [RequireComponent(typeof(FighterFSM_Controller))]
    public class FighterDrone : Drone
    {
        Anchor fightAnchor;
        [SerializeField] float refractoryTime = 1f;
        bool canFire = true;

        protected override void Awake()
        {
            base.Awake();
            canFire = true;
        }

        public override void SetQueen(Queen queenie)
        {
            base.SetQueen(queenie);
            fightAnchor = queenie.fightAnchor;
            fightAnchor.PlaceAnchor += SetDestination;
        }

        public override void Death()
        {
            StopAllCoroutines();
            if (fightAnchor != null )
            {
                fightAnchor.PlaceAnchor -= SetDestination;
                fightAnchor = null;
            }
            base.Death();
        }
        public override Vector2 AnchorPoint()
        {
            return fightAnchor.IsActive() ? fightAnchor.GetLocation() : myQueen.Location();
        }

        public void Attack(Unit other)
        {
            if (!canFire)
                return;

            other.Death(); // Change to TakeDamage(value)
            canFire = false;
            if(gameObject.activeSelf)
            {
                StartCoroutine(RefractoryPeriod());
            }
        }

        IEnumerator RefractoryPeriod()
        {
            yield return new WaitForSeconds(refractoryTime);
            canFire = true;
        }
    }
}