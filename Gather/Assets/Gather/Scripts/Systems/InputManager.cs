using UnityEngine;

namespace Gather
{
    public class InputManager : MonoBehaviour
    {
        Queen player;
        Anchor activeAnchor;
        RaycastHit2D hit;
        [SerializeField] LayerMask anchorMask;

        void Update()
        {
           
            if (Input.GetButtonDown(Inputs.LeftClick))
            {
                PlaceAnchor();
            }
                
            if (Input.GetButtonDown(Inputs.RightClick))
            {
                 RemoveAnchor();
            }
            
            if (Input.GetButtonDown(Inputs.SpawnFarmer))
            {
                player.SpawnFarmer();
            }

            if (Input.GetButtonDown(Inputs.SpawnFighter))
            {
                player.SpawnFighter();
            }
        }

        public void SetPlayer(Queen player)
        {
            this.player = player;
            player.SetInputManager(this);
        }

        public void SetActiveAnchor(Anchor activeAnchor)
        {
            this.activeAnchor = activeAnchor;
        }

        //public void ToggleFoodAnchor()
        //{
        //    SetActiveAnchor(foodAnchor);
        //    foodAnchor.SetReadyToPlace();
        //}

        //public void ToggleFightAnchor()
        //{

        //}

        void PlaceAnchor()
        {
            if(activeAnchor && activeAnchor.IsReadyToPlace()) 
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f);

                if (hit)
                {
                    activeAnchor.SetAnchorPoint(hit.point);
                }
            }
        }

        void RemoveAnchor()
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100f, anchorMask);
            if(hit)
            {
                Debug.Log(hit.transform);

                Anchor selectedAnchor = hit.collider.GetComponent<Anchor>();
                if(selectedAnchor)
                {
                    selectedAnchor.Deactivate();
                }
            }
        }
    }
}