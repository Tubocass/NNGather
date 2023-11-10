using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (SceneManager.GetSceneByName("UI").isLoaded == false)
                SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
            else
                SceneManager.UnloadSceneAsync("UI");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (SceneManager.GetSceneByName("Level2").isLoaded)
                SceneManager.UnloadSceneAsync("Level2");

            if (SceneManager.GetSceneByName("Level1").isLoaded == false)
            {
                SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive).completed += operation => 
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level1"));
            }
        }
        
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (SceneManager.GetSceneByName("Level1").isLoaded)
                SceneManager.UnloadSceneAsync("Level1");

            if (SceneManager.GetSceneByName("Level2").isLoaded == false)
            {
                SceneManager.LoadSceneAsync("Level2", LoadSceneMode.Additive)
                    .completed += HandleLevel2LoadCompleted;
            }
        }
    }

    private void HandleLevel2LoadCompleted(AsyncOperation obj)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Level2"));
    }
}