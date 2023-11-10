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
                Drone enemy = collision.GetComponent<Drone>();
                if(enemy.GetTeam() != parentDrone.GetTeam())
                {
                    enemy.Death();
                }
            }
        }
    }
}