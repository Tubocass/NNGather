using UnityEngine;
using System.Collections.Generic;

namespace gather
{
    public class FoodPellet : MonoBehaviour, ITarget
    {
        Collider2D myCollider;
        Transform myTransform;
        bool isPickedUp = false;
        //bool isTargeted;
        //Dictionary<int, int> targetedByTeam = new Dictionary<int, int>();

        //public void Targeted(Unit drone)
        //{
        //    if (!targetedByTeam.ContainsKey(drone.GetTeam()))
        //    {
        //        targetedByTeam.Add(drone.GetTeam(), drone.GetInstanceID());
        //    }
        //}
        //public void UnTargeted(Unit drone)
        //{
        //    if (targetedByTeam.ContainsKey(drone.GetTeam()))
        //    {
        //        targetedByTeam.Remove(drone.GetTeam());
        //    }
        //}
        //public bool CanTarget(Unit drone)
        //{
        //    int heldID;
        //    targetedByTeam.TryGetValue(drone.GetTeam(), out heldID);
        //    isTargeted = targetedByTeam.ContainsKey(drone.GetTeam()) && !(heldID == drone.GetInstanceID());
        //    return gameObject.activeSelf && !(isTargeted || isPickedUp);
        //}

        public Vector2 Location()
        {
            return myTransform.position;
        }

        private void Awake()
        {
            myCollider = GetComponent<Collider2D>();
            myTransform = this.transform;
        }
    
        private void OnEnable()
        {
            myCollider.enabled = true;
        }

        public bool CanTarget(int team)
        {
            return gameObject.activeSelf && !(isPickedUp);
        }

        public void Attach(Transform newParent)
        {
            myTransform.SetParent(newParent);
            myCollider.enabled = false;
            isPickedUp = true;
            //targetedByTeam.Clear();
        }

        public void Detach()
        {
            myTransform.SetParent(null);
            isPickedUp = false;
        }

        public void Consume()
        {
            Detach();
            gameObject.SetActive(false);
        }
    }
}
