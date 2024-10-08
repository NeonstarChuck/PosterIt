using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance; // Singleton instance

    public GameObject menuUIPrefab; // Reference to the menu UI prefab

    public Button titelButton; // Button to return to main menu
    public Button beställaPosterButton; // Button to order posters
    public Button skapaEgenPosterButton; // Button to create custom poster
    public Button kontaktButton; // Button for contact information
    public Button tillKassanButton; // Button to go to checkout

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object when loading new scenes
            LoadMenuUI(); // Load the Menu UI when this object is created
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    void Start()
    {
        // Connect button functions
        beställaPosterButton.onClick.AddListener(ÖppnaBeställaPoster);
        skapaEgenPosterButton.onClick.AddListener(ÖppnaSkapaEgenPoster);
        kontaktButton.onClick.AddListener(ÖppnaKontakt);
        tillKassanButton.onClick.AddListener(ÖppnaKassa);

        // Connect title button
        titelButton.onClick.AddListener(TillHuvudmeny);
    }

    void LoadMenuUI()
    {
        if (menuUIPrefab != null)
        {
            GameObject menuUI = Instantiate(menuUIPrefab); // Instantiate the menu UI prefab
            menuUI.transform.SetParent(transform, false); // Set parent to this object without affecting layout
        }
    }

    void ÖppnaBeställaPoster()
    {
        Debug.Log("Öppnar Beställa Poster Scen");
        SceneManager.LoadScene("BeställaPosterScen");
    }

    void ÖppnaSkapaEgenPoster()
    {
        Debug.Log("Öppnar Skapa Egen Poster Scen");
        SceneManager.LoadScene("SkapaEgenPosterScen");
    }

    void ÖppnaKontakt()
    {
        Debug.Log("Öppnar Kontakt Scen");
        SceneManager.LoadScene("KontaktScen");
    }

    void ÖppnaKassa()
    {
        Debug.Log("Öppnar Kassa Scen");
        SceneManager.LoadScene("KassaScen");
    }

    void TillHuvudmeny()
    {
        Debug.Log("Återgår till Huvudmeny");

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "MainMenuScene")
        {
            // If already in Main Menu, refresh the scene
            SceneManager.LoadScene(currentSceneName);
        }
        else
        {
            // Otherwise, load the Main Menu scene
            SceneManager.LoadScene("MainMenuScene");
        }
        
        // Optionally, reset any necessary UI elements here if needed.
    }
}
