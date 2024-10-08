using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UniversalUIManager : MonoBehaviour
{
    public Button beställaPosterButton; // Button to go to Beställa Poster Scene
    public Button skapaEgenPosterButton; // Button to go to Skapa Egen Poster Scene
    public Button kontaktButton; // Button to go to Kontakt Scene
    public Button kassaButton; // Button to go to Kassa Scene
    public Button titelButton; // Button to return to main menu

    private void Start()
    {
        // Connect each scene button to its respective method
        if (beställaPosterButton != null)
        {
            beställaPosterButton.onClick.AddListener(() => LoadScene("BeställaPosterScene"));
        }

        if (skapaEgenPosterButton != null)
        {
            skapaEgenPosterButton.onClick.AddListener(() => LoadScene("SkapaEgenPosterScene"));
        }

        if (kontaktButton != null)
        {
            kontaktButton.onClick.AddListener(() => LoadScene("KontaktScen"));
        }

        if (kassaButton != null)
        {
            kassaButton.onClick.AddListener(() => LoadScene("KassaScen"));
        }

        // Connect the title button to return to main menu
        if (titelButton != null)
        {
            titelButton.onClick.AddListener(TillHuvudmeny);
        }
    }

    void LoadScene(string sceneName)
    {
        Debug.Log("Laddar scen: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    void TillHuvudmeny()
    {
        Debug.Log("Återgår till Huvudmeny");
        SceneManager.LoadScene("MainMenuScene"); // Load the main menu scene
    }
}
