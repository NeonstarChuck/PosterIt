using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARManager : MonoBehaviour
{
    public GameObject imagePrefab;
    public float imageSize = 1.2f; // 120cm

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private bool isImagePlaced = false;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    void Update()
    {
        if (!isImagePlaced && planeManager.trackables.count > 0)
        {
            PlaceImage();
        }
    }

    void PlaceImage()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            Pose hitPose = hits[0].pose;
            GameObject placedImage = Instantiate(imagePrefab, hitPose.position, hitPose.rotation);
            placedImage.transform.localScale = Vector3.one * imageSize;
            isImagePlaced = true;

            // Optionally, disable plane detection after placing the image
            planeManager.enabled = false;
        }
    }
}