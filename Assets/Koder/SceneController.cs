using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void GoToPreviousScene()
    {
        // Get the index of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Load the previous scene (assuming it's the one before in the build order)
        SceneManager.LoadScene(currentSceneIndex - 1);
    }
}