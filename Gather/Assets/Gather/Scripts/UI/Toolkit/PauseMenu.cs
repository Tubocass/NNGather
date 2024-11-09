using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Gather.UI.Toolkit
{
    public class PauseMenu : MonoBehaviour
    {
        bool gamePaused = false;
        VisualElement pauseMenu;

        private void Start()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            pauseMenu = root.Q<VisualElement>(name: "pause_menu");
            Button newGame = pauseMenu.Q<Button>(name: "new_game_button");
            Button options = pauseMenu.Q<Button>(name: "options_button");
            Button exitGame = pauseMenu.Q<Button>(name: "exit_button");
            newGame.clicked += NewGame;
            //options.clicked += DisplayOptions;
            exitGame.clicked += ExitGame;

        }

        public void NewGame()
        {
            Time.timeScale =  1f;
            SceneManager.LoadScene("NewGameScreen");
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
            }
        }

        public void TogglePauseMenu()
        {
            gamePaused = !gamePaused;
            Time.timeScale = gamePaused ? 0f : 1f;
            pauseMenu.style.visibility = gamePaused ? Visibility.Visible : Visibility.Hidden;

        }
    }
}