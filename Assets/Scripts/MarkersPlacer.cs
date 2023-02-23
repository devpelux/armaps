using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MarkersPlacer : MonoBehaviour
{
    public GameObject markerPrefab;

    private ARRaycastManager raycastManager;
    private ARAnchorManager anchorManager;

    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose pose = hits[0].pose;
                    ARPlane plane = (ARPlane) hits[0].trackable;

                    _ = AttachAnchor(plane, pose, markerPrefab);
                }
            }
        }
    }

    /// <summary>
    /// Crea un ancora con il prefab specificato nel piano e posizione specificati.
    /// Se il prefab è null, verrà creata un ancora di default.
    /// </summary>
    private ARAnchor AttachAnchor(ARPlane plane, Pose pose, GameObject prefab)
    {
        if (prefab != null)
        {
            GameObject oldPrefab = anchorManager.anchorPrefab;
            anchorManager.anchorPrefab = prefab;
            ARAnchor anchor = anchorManager.AttachAnchor(plane, pose);
            anchorManager.anchorPrefab = oldPrefab;
            return anchor;
        }
        else
        {
            return anchorManager.AttachAnchor(plane, pose);
        }
    }
}
