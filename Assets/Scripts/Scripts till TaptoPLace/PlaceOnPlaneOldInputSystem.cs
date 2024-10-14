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

        // Load the poster size selected in the previous scene
        float width = PlayerPrefs.GetFloat("PosterWidth", 0.7f);
        float height = PlayerPrefs.GetFloat("PosterHeight", 0.7f);
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
            var hitPose = hits[0].pose;
            var trackableId = hits[0].trackableId;
            var plane = arPlaneManager.GetPlane(trackableId);

            if (plane != null && IsWallPlane(plane))
            {
                if (spawnedObject == null)
                {
                    // First placement
                    spawnedObject = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                    SetPosterScale(spawnedObject);
                    spawnedObject.transform.forward = plane.normal * -1f; // Use multiplication instead of negation
                    hasPlacedPoster = true;
                    StopPlaneDetectionAndHidePlanes();
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
            spawnedObject.transform.forward = hitPose.rotation * Vector3.back; // Use Vector3.back instead of negating
        }

        if (touch.phase == TouchPhase.Ended)
        {
            isMoving = false;
        }
    }

    bool IsWallPlane(ARPlane plane)
    {
        // Check if the plane's normal is roughly horizontal
        return Vector3.Dot(plane.normal, Vector3.up) < 0.1f;
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
