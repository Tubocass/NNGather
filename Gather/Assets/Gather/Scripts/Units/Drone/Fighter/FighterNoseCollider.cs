using System.Collections;
using UnityEngine;

namespace gather
{
    public class FighterNoseCollider : MonoBehaviour
    {
        FighterDrone parentDrone;

        private void Awake()
        {
            parentDrone = GetComponentInParent<FighterDrone>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.units)) {
                Unit enemy = collision.GetComponent<Unit>();
                if(enemy.GetTeam() != parentDrone.GetTeam())
                {
                    parentDrone.Attack(enemy);
                }
            }
        }
    }
}