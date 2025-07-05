using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Loads a scene by its name.
    /// This method is public so it can be called by UI buttons.
    /// </summary>
    /// <param name="sceneName">The exact name of the scene to load.</param>
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is empty or null. Cannot load scene.");
            return;
        }

        Debug.Log("Loading Scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitApplication()
    {
        Debug.Log("Quitting Application...");
        Application.Quit();
    }
}