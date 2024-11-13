using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

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
    bool isMoving = false;
    bool hasPlacedPoster = false;

    void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();

        // Set plane detection mode to vertical
        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Vertical;

        // Load the poster size selected in the previous scene
        float width = PlayerPrefs.GetFloat("PosterWidth", 0.6f);  // Default to 60x90 cm if no selection is made
        float height = PlayerPrefs.GetFloat("PosterHeight", 0.9f); // Default to 60x90 cm if no selection is made
        posterSize = new Vector2(width, height);
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (hasPlacedPoster)
        {
            HandlePosterMovement(touch);
            return;
        }

        if (aRRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            foreach (var hit in hits)
            {
                var plane = arPlaneManager.GetPlane(hit.trackableId);

                if (plane != null && IsWallPlane(plane))
                {
                    if (spawnedObject == null)
                    {
                        // First placement
                        spawnedObject = Instantiate(placedPrefab, hit.pose.position, hit.pose.rotation);
                        SetPosterScale(spawnedObject);
                        spawnedObject.transform.forward = plane.normal * -1f;
                        hasPlacedPoster = true;
                        StopPlaneDetectionAndHidePlanes();
                        
                        if (Debug.isDebugBuild)
                        {
                            Debug.Log("Poster placed on wall");
                        }
                        
                        break; // Exit the loop after placing the poster
                    }
                }
            }
        }
    }

    void HandlePosterMovement(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            // Check if we're touching the spawned object
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (spawnedObject != null && spawnedObject.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
            {
                isMoving = true;
            }
        }

        if (isMoving && aRRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            spawnedObject.transform.position = hitPose.position;
            spawnedObject.transform.forward = hitPose.rotation * Vector3.back;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            isMoving = false;
        }
    }

    bool IsWallPlane(ARPlane plane)
    {
        // Check if the plane's normal is close to horizontal (wall)
        float angleThreshold = 5f; // Degrees
        float angle = Vector3.Angle(plane.normal, Vector3.up);
        
        // A wall should be close to 90 degrees from the up vector
        bool isWall = angle > (90 - angleThreshold) && angle < (90 + angleThreshold);
        
        if (isWall && Debug.isDebugBuild)
        {
            Debug.Log($"Detected wall plane. Angle: {angle}");
        }
        
        return isWall;
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
    }

    public GameObject GetSpawnedObject()
    {
        return spawnedObject;
    }
}
