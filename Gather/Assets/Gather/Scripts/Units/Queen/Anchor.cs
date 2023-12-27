using UnityEngine;

namespace gather
{
    public class Anchor : MonoBehaviour
    {
        public LocationEvent PlaceAnchor;
        Transform myTransform;
        bool active;

        private void Awake()
        {
            myTransform = transform;
        }
        public bool GetActive()
        {
            return active;
        }

        public void SetActive(bool value)
        {
            active = value;
            gameObject.SetActive(value);
        }

        public Vector2 GetPosition()
        {
            return myTransform.position;
        }

        public void SetPosition(Vector2 value)
        {
            myTransform.position = value;
        }
    }
}