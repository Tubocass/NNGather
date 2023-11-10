using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gather
{
    public class FoodPellet : MonoBehaviour, ITarget
    {
        Collider2D myCollider;
        Transform myTransform;
        Dictionary<int, bool> targetedByTeam = new Dictionary<int, bool>();
        bool isPickedUp = false;
        bool isTargeted = false;

        public Vector3 Location()
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
        public void Targeted(int team, bool target)
        {
            if (targetedByTeam.ContainsKey(team))
            {
                targetedByTeam[team] = target;
            } else
            {
                targetedByTeam.Add(team, target);
            }
        }
        public bool CanTarget(int team)
        {
            isTargeted = targetedByTeam.TryGetValue(team, out isTargeted);
            return gameObject.activeSelf && !(isTargeted || isPickedUp);
        }

        public void Attach(Transform newParent)
        {
            myTransform.SetParent(newParent);
            myCollider.enabled = false;
            isPickedUp = true;
            targetedByTeam.Clear();
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
