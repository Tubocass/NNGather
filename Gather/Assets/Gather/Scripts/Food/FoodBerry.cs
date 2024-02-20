using UnityEngine;

namespace gather
{
    [RequireComponent(typeof(PooledObject))]
    public class FoodBerry : MonoBehaviour, ITargetable
    {
        Collider2D myCollider;
        Transform myTransform;
        bool isPickedUp = false;
        PooledObject po;
        FoodBush parentBush;
        public FoodBush ParentBush => parentBush;

        private void Awake()
        {
            myCollider = GetComponent<Collider2D>();
            myTransform = this.transform;
            po = GetComponent<PooledObject>();
        }

        public void SetParentBush(FoodBush bush)
        {
            parentBush = bush;
        }
    
        private void OnEnable()
        {
            myCollider.enabled = true;
        }

        public Vector2 GetLocation()
        {
            return myTransform.position;
        }

        public bool CanBeTargeted(int team)
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
            myCollider.enabled = true;
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