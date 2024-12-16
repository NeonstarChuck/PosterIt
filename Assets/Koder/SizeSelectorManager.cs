using UnityEngine;
using UnityEngine.UI;

public class SizeSelectorManager : MonoBehaviour
{
    public Button size70x100Button; // Button for 70x100 size
    public Button size50x70Button; // Button for 50x70 size
    public Button size30x40Button; // Button for 30x40 size
    public GameObject placedPrefab; // The prefab that will be resized

    private Button currentlySelectedButton;

    // Colors for button states
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    private void Start()
    {
        size70x100Button.onClick.AddListener(() => OnButtonClicked(size70x100Button, 0.7f, 1.0f));
        size50x70Button.onClick.AddListener(() => OnButtonClicked(size50x70Button, 0.5f, 0.7f));
        size30x40Button.onClick.AddListener(() => OnButtonClicked(size30x40Button, 0.3f, 0.4f));

        // Initialize button colors
        ResetAllButtonColors();
    }

    private void OnButtonClicked(Button clickedButton, float width, float height)
    {
        if (currentlySelectedButton != null)
        {
            SetButtonColor(currentlySelectedButton, normalColor); // Reset the previous button's color
        }

        // Select the new button
        currentlySelectedButton = clickedButton;
        SetButtonColor(clickedButton, selectedColor);

        // Save poster size
        PlayerPrefs.SetFloat("PosterWidth", width);
        PlayerPrefs.SetFloat("PosterHeight", height);

        // Resize the instantiated poster
        if (placedPrefab != null)
        {
            ResizePoster(width, height);
        }
    }

    private void SetButtonColor(Button button, Color color)
    {
        if (button != null)
        {
            ColorBlock cb = button.colors;  // Access the button's ColorBlock
            cb.normalColor = color;        // Update the normal color
            cb.highlightedColor = color;   // Update the highlighted color (optional)
            cb.selectedColor = color;      // Update the selected color (optional)
            button.colors = cb;            // Apply the changes
        }
    }

    private void ResetAllButtonColors()
    {
        // Reset all buttons to the normal color
        SetButtonColor(size70x100Button, normalColor);
        SetButtonColor(size50x70Button, normalColor);
        SetButtonColor(size30x40Button, normalColor);
    }

    private void ResizePoster(float width, float height)
    {
        // Assuming the 'placedPrefab' is the current instance of the poster
        if (placedPrefab != null)
        {
            Vector3 newScale = new Vector3(width, height, placedPrefab.transform.localScale.z);
            placedPrefab.transform.localScale = newScale;
        }
    }
}
