﻿using System.Collections;
using UnityEngine;

namespace Gather
{
    public class FarmerNoseCollider : MonoBehaviour
    {
        FarmerDrone parentDrone;
        Vector3 relativePosition = new Vector3(0, 0.6f, 0);

        private void Awake()
        {
            parentDrone = GetComponentInParent<FarmerDrone>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (parentDrone.IsCarryingFood())
            {
                if (collision.gameObject.CompareTag(Tags.queen))
                {
                    parentDrone.DropoffFood(collision.gameObject.GetComponent<Queen>());
                }
            }
            else if (collision.gameObject.CompareTag(Tags.food))
            {

                FoodBerry pellet = collision.gameObject.GetComponent<FoodBerry>();
                if (pellet.CanBeTargeted(parentDrone.GetTeamID()))
                {
                    pellet.Attach(this.transform);
                    pellet.transform.localPosition = relativePosition;
                    parentDrone.PickupFood(pellet);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (parentDrone.IsCarryingFood())
            {
                if (collision.gameObject.CompareTag(Tags.queen))
                {
                    parentDrone.DropoffFood(collision.gameObject.GetComponent<Queen>());
                }
            }
        }
    }
}