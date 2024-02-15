using UnityEngine;

namespace gather
{
    [RequireComponent(typeof(PooledObject))]
    public class FoodPellet : MonoBehaviour, ITargetable
    {
        Collider2D myCollider;
        Transform myTransform;
        bool isPickedUp = false;
        PooledObject po;

        private void Awake()
        {
            myCollider = GetComponent<Collider2D>();
            myTransform = this.transform;
            po = GetComponent<PooledObject>();
        }
    
        private void OnEnable()
        {
            myCollider.enabled = true;
        }

        public Vector2 GetLocation()
        {
            return myTransform.position;
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
            po.ReleaseToPool();
        }
    }
}