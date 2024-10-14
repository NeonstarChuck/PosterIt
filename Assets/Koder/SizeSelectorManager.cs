using UnityEngine;
using UnityEngine.UI;

public class SizeSelectorManager : MonoBehaviour
{
    public Button size90x70Button;
    public Button size75x50Button;
    public Button size120x90Button;

    private Button currentlySelectedButton;
    
    // Colors for button states
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    private void Start()
    {
        size90x70Button.onClick.AddListener(() => OnButtonClicked(size90x70Button, 0.9f, 0.7f));
        size75x50Button.onClick.AddListener(() => OnButtonClicked(size75x50Button, 0.75f, 0.5f));
        size120x90Button.onClick.AddListener(() => OnButtonClicked(size120x90Button, 1.2f, 0.9f));

        // Initialize all buttons to normal color
        SetButtonColor(size90x70Button, normalColor);
        SetButtonColor(size75x50Button, normalColor);
        SetButtonColor(size120x90Button, normalColor);
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
