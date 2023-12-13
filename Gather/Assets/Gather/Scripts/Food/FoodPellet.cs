using UnityEngine;

namespace gather
{
    public class FoodPellet : MonoBehaviour, ITarget
    {
        Collider2D myCollider;
        Transform myTransform;
        bool isPickedUp = false;

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
