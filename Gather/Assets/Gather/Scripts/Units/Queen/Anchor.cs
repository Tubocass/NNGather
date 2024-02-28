using UnityEngine;

namespace Gather
{
    public class Anchor : MonoBehaviour
    {
        public LocationEvent PlaceAnchor;
        Transform myTransform;
        bool active;
        bool placing;

        private void Awake()
        {
            myTransform = transform;
        }

        private void Update()
        {
            if (placing) 
            {
                myTransform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

        public bool IsActive()
        {
            return active;
        }

        public bool IsReadyToPlace()
        {
            return placing;
        }

        public void SetReadyToPlace()
        {
            active = false;
            placing = true;
            gameObject.SetActive(true);
        }

        public void Deactivate() 
        {
            active = false;
            gameObject.SetActive(false);
        }

        public Vector2 GetLocation()
        {
            return myTransform.position;
        }

        public void SetAnchorPoint(Vector2 location)
        {
            placing = false;
            active = true;
            myTransform.position = location;
            PlaceAnchor?.Invoke(location);
        }
    }
}