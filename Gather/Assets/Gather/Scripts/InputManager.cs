using UnityEngine;

namespace gather
{
    public class InputManager : MonoBehaviour
    {
        Queen player;
        RaycastHit2D hit;

        public void SetPlayer(Queen player)
        {
            this.player = player;
        }

        void Update()
        {
            if(Input.GetButtonDown(Inputs.LeftClick))
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint( Input.mousePosition), Vector2.zero, 100f);
                
                if (hit)
                {
                    Debug.Log(hit.point);
                    player.PlaceFightAnchor(hit.point);
                }
            }
            if (Input.GetButtonDown(Inputs.RightClick))
            {
                Debug.Log("Right");
                player.RemoveFightAnchor();
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
    }
}