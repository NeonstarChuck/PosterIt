using UnityEngine;
using UnityEngine.UI;

public class SizeSelectorManager : MonoBehaviour
{
    public Button size120x90Button;  // Button for 120x90 size
    public Button size90x70Button;   // Button for 90x70 size
    public Button size75x50Button;   // Button for 75x50 size

    private Button currentlySelectedButton;
    
    // Colors for button states
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    private void Start()
    {
        size120x90Button.onClick.AddListener(() => OnButtonClicked(size120x90Button, 0.6f, 0.9f)); // 60x90 cm
        size90x70Button.onClick.AddListener(() => OnButtonClicked(size90x70Button, 0.4f, 0.6f));   // 40x60 cm
        size75x50Button.onClick.AddListener(() => OnButtonClicked(size75x50Button, 0.2f, 0.3f));   // 20x30 cm

        // Initialize all buttons to normal color
        SetButtonColor(size120x90Button, normalColor);
        SetButtonColor(size90x70Button, normalColor);
        SetButtonColor(size75x50Button, normalColor);
    }

    private void OnButtonClicked(Button clickedButton, float width, float height)
    {
        if (currentlySelectedButton != null && currentlySelectedButton != clickedButton)
        {
            // Reset the color of the previously selected button
            SetButtonColor(currentlySelectedButton, normalColor);
        }

        // Set the color of the clicked button
        SetButtonColor(clickedButton, selectedColor);

        // Update the currently selected button
        currentlySelectedButton = clickedButton;

        // Set the poster size
        PlayerPrefs.SetFloat("PosterWidth", width);
        PlayerPrefs.SetFloat("PosterHeight", height);
    }

    private void SetButtonColor(Button button, Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.selectedColor = color;
        button.colors = colors;
    }
}
