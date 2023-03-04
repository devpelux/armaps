using ARMaps.ARObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARMaps
{
    public class MarkersPlacer : MonoBehaviour
    {
        public GameObject markerPrefab;

        private ARRaycastManager raycastManager;
        private ARAnchorManager anchorManager;

        private static readonly List<ARRaycastHit> hits = new();

        private int markerCount = 0;

        void Awake()
        {
            raycastManager = GetComponent<ARRaycastManager>();
            anchorManager = GetComponent<ARAnchorManager>();
        }

        void Update()
        {
            if (markerPrefab != null && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose pose = hits[0].pose;
                        ARPlane plane = (ARPlane)hits[0].trackable;

                        ARAnchor marker = AnchorMarker(plane, pose, markerPrefab);

                        //Scosta il tempo così da avere animazioni progressive tra gli oggetti.
                        marker.GetComponent<PathMarker>().waveDelay = 0.2f * markerCount;

                        markerCount++;
                    }
                }
            }
        }

        /// <summary>
        /// Crea un ancora con il marker specificato nel piano e posizione specificati.
        /// Restituisce l'oggetto creato.
        /// </summary>
        private ARAnchor AnchorMarker(ARPlane plane, Pose pose, GameObject marker)
        {
            GameObject anchorPrefab = anchorManager.anchorPrefab;
            anchorManager.anchorPrefab = marker;
            ARAnchor anchor = anchorManager.AttachAnchor(plane, pose);
            anchorManager.anchorPrefab = anchorPrefab;
            return anchor;
        }
    }
}
