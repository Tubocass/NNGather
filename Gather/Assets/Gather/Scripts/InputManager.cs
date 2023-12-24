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
            if(Input.GetButtonDown(Inputs.LeftClick))
            {
                Debug.Log("Left");
            }
            if (Input.GetButtonDown(Inputs.RightClick))
            {
                Debug.Log("Right");
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