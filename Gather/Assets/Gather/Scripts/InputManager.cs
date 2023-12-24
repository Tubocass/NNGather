using UnityEngine;

namespace gather
{
    public class InputManager : MonoBehaviour
    {
        PlayerQueen player;

        public void SetPlayer(PlayerQueen player)
        {
            this.player = player;
        }

        void Update()
        {
            if(Input.GetButtonDown("LeftClick"))
            {
                Debug.Log("Left");
            }
            if (Input.GetButtonDown("MiddleClick"))
            {
                Debug.Log("Middle");
            }
            if (Input.GetButtonDown("RightClick"))
            {
                Debug.Log("Right");
            }
            if (Input.GetButtonDown("SpawnFarmer"))
            {
                player.SpawnFarmer();
            }
            if (Input.GetButtonDown("SpawnFighter"))
            {
                player.SpawnFighter();
            }
        }
    }
}