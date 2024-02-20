using UnityEngine;
using System.Collections;

namespace gather
{
    public class AggressiveNoseCollider : MonoBehaviour
    {
        Unit parentDrone;
        [SerializeField] float refractoryTime = 1f;
        [SerializeField] int damage = 1;
        bool canFire = true;

        private void Awake()
        {
            parentDrone = GetComponentInParent<Unit>();
            canFire = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.units)) {
                Unit enemy = collision.GetComponent<Unit>();
                if(enemy.CanBeTargeted(parentDrone.GetTeamID()))
                {
                    Attack(enemy.Health);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.units))
            {
                Unit enemy = collision.GetComponent<Unit>();
                if (enemy.CanBeTargeted(parentDrone.GetTeamID()))
                {
                    Attack(enemy.Health);
                }
            }
        }

        public void Attack(Health other)
        {
            if (!canFire)
                return;

            other.TakeDamage(damage);
            canFire = false;
            if (parentDrone.isActiveAndEnabled)
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