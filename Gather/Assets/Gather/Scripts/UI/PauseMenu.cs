using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gather.UI
{
    public class PauseMenu : MonoBehaviour
    {
        bool gamePaused = false;

        public void ExitGame()
        {
            Application.Quit();
        }

        public void NewGame()
        {
            Time.timeScale =  1f;
            SceneManager.LoadScene("NewGameScreen");
        }

        public void TogglePauseMenu()
        {
            gamePaused = !gamePaused;
            Time.timeScale = gamePaused ? 0f : 1f;
            gameObject.SetActive(gamePaused);
        }
    }
}