using UnityEngine;
using UnityEngine.SceneManagement;

public class ARLauncher : MonoBehaviour
{
    public void LaunchAR()
    {
        SceneManager.LoadScene("ARScene");
    }
}