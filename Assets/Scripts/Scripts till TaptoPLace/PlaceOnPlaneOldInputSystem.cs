using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using TMPro; // Required for TextMeshPro

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnWallOldInputSystem : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a wall at the touch location.")]
    GameObject placedPrefab;

    public Vector2 posterSize;

    GameObject spawnedObject;
    ARRaycastManager aRRaycastManager;
    ARPlaneManager arPlaneManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Cached reference to the UI text component
    TextMeshProUGUI detectionText;

    void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();

        // Set plane detection mode to vertical
        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Vertical;

        // Load the poster size selected in the previous scene
        float width = PlayerPrefs.GetFloat("PosterWidth", 0.5f);  // Default to 60x90 cm if no selection is made
        float height = PlayerPrefs.GetFloat("PosterHeight", 0.7f); // Default to 60x90 cm if no selection is made
        posterSize = new Vector2(width, height);

        // Find the UI text component in the scene
        detectionText = GameObject.Find("DetectionText").GetComponent<TextMeshProUGUI>();

        if (detectionText == null)
        {
            Debug.LogError("UI Text component not found in the scene");
        }

        // Set initial text to "Finding..."
        SetDetectionText("Finding...");
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (spawnedObject != null)
        {
            HandlePosterMovement(touch);
        }
        else
        {
            TryPlacePoster(touch);
        }
    }

    void TryPlacePoster(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            hits.Clear();
            if (aRRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                foreach (var hit in hits)
                {
                    var plane = arPlaneManager.GetPlane(hit.trackableId);

                    if (plane != null && IsWallPlane(plane))
                    {
                        // Update UI text to "Found Plane" as soon as a valid plane is detected
                        SetDetectionText("Found Plane");

                        spawnedObject = Instantiate(placedPrefab, hit.pose.position, hit.pose.rotation);
                        SetPosterScale(spawnedObject);
                        spawnedObject.transform.forward = plane.normal * -1f; // Ensure it faces the wall
                        StopPlaneDetectionAndHidePlanes();

                        Debug.Log("Poster placed on wall");
                        break; // Exit the loop after placing the poster
                    }
                }
            }
            else
            {
                Debug.Log("No planes detected at touch position");
            }
        }
    }

    void HandlePosterMovement(Touch touch)
    {
        if (touch.phase == TouchPhase.Moved)
        {
            hits.Clear();
            if (aRRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                var hit = hits[0]; // Use the first hit result
                var plane = arPlaneManager.GetPlane(hit.trackableId);

                if (plane != null && IsWallPlane(plane))
                {
                    // Update the poster's position
                    spawnedObject.transform.position = hit.pose.position;

                    // Ensure it faces the wall
                    spawnedObject.transform.forward = plane.normal * -1f;

                    Debug.Log("Poster moved to a new position");
                }
            }
        }
    }

    bool IsWallPlane(ARPlane plane)
    {
        // Check if the plane's normal is close to horizontal (wall)
        float angleThreshold = 5f; // Degrees
        float angle = Vector3.Angle(plane.normal, Vector3.up);

        return angle > (90 - angleThreshold) && angle < (90 + angleThreshold);
    }

    void SetPosterScale(GameObject poster)
    {
        // Set the scale to ensure the poster is the specified size
        poster.transform.localScale = new Vector3(posterSize.x, posterSize.y, 0.01f);
    }

    void StopPlaneDetectionAndHidePlanes()
    {
        // Stop plane detection
        arPlaneManager.enabled = false;

        // Hide all existing planes
        foreach (var plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        Debug.Log("Plane detection stopped and planes hidden");
    }

    void SetDetectionText(string message)
    {
        if (detectionText != null)
        {
            detectionText.text = message;
        }
        else
        {
            Debug.LogError("UI Text component not found, unable to set detection text");
        }
    }

    public GameObject GetSpawnedObject()
    {
        return spawnedObject;
    }
}
