using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    public ScrollRect scrollRect;

    private Vector2 lastTouchPosition;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    Vector2 touchDeltaPosition = touch.position - lastTouchPosition;
                    scrollRect.content.anchoredPosition += touchDeltaPosition / 10; // Adjust sensitivity as needed
                    lastTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    break;
            }
        }
    }
}
