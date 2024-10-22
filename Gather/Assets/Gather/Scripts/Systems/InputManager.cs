using UnityEngine;

namespace Gather
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] LayerMask anchorMask;
        Anchor foodAnchor, fightAnchor;
        Queen player;
        Anchor activeAnchor;
        RaycastHit2D hit;

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
        public void SetAnchors(Anchor foodAnchor, Anchor fightAnchor)
        {
            // given by GameController
            this.foodAnchor = foodAnchor;
            this.fightAnchor = fightAnchor;
        }

        public void SetPlayer(Queen player)
        {
            this.player = player;
            player.SetAnchors(foodAnchor, fightAnchor);
        }

        public void SetActiveAnchor(Anchor activeAnchor)
        {
            this.activeAnchor = activeAnchor;
        }

        public void ToggleFoodAnchor()
        {
            if (foodAnchor.IsActive())
            {
                foodAnchor.Deactivate();
            } else
            {
                ReadyFoodAnchor();
            }
        }

        public void ToggleFightAnchor()
        {
            if (fightAnchor.IsActive())
            {
                fightAnchor.Deactivate();
            } else
            {
                ReadyFightAnchor();
            }
        }

        public void ReadyFoodAnchor()
        {
            SetActiveAnchor(foodAnchor);
            foodAnchor.SetReadyToPlace();
        }

        public void ReadyFightAnchor()
        {
            SetActiveAnchor(fightAnchor);
            fightAnchor.SetReadyToPlace();
        }

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