using ARMaps.ARObjects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace ARMaps
{
    /// <summary>
    /// Oggetto che si occupa di gestire l'interazione con il mondo esterno, in particolare il piazzamento dei marcatori di percorso.
    /// I marcatori devono trovarsi sul layer 6 (Marker) per potervi interagire.
    /// </summary>
    [RequireComponent(typeof(ARRaycastManager))]
    public class MarkerManager : MonoBehaviour
    {
        [Tooltip("Camera principale.")]
        [SerializeField] private Camera mainCamera;

        [Tooltip("Prefab dei marker.")]
        [SerializeField] private GameObject[] markerPrefabs;

        [Tooltip("Latenza tra il tocco sullo schermo e la risposta. È usata per registrare i tocchi multipli.")]
        [Range(0.01f, 1f)]
        public float touchLatence = 0.05f;

        [Tooltip("Durata del tocco per la cancellazione di un marker e i successivi.")]
        [Range(0.1f, 50f)]
        public float maxInteractionDistance = 5f;

        /// <summary>
        /// Layer mask dei marker (layer 6).
        /// </summary>
        public const int MARKER_LAYER_MASK = 1 << 6;

        /// <summary>
        /// Restituisce il numero di marker attualmente presenti.
        /// </summary>
        public int MarkerCount => markerObjects.Count;

        /// <summary>
        /// Restituisce la posizione attuale della camera principale.
        /// </summary>
        public Vector3 MainCameraPosition => mainCamera.gameObject.transform.position;

        /// <summary>
        /// Contiene le posizioni dei marker attualmente piazzati.
        /// </summary>
        private readonly List<Vector3> markerPositions = new();

        /// <summary>
        /// Contiene i gameobject dei marker attualmente piazzati.
        /// </summary>
        private readonly List<GameObject> markerObjects = new();

        /// <summary>
        /// Indica se sono abilitate le interazioni con i marker.
        /// </summary>
        private bool enableInteractions = false;

        /// <summary>
        /// Conserva l'istante di tempo in secondi in cui è stato toccato lo schermo l'ultima volta.
        /// </summary>
        private float lastTouchTime;

        /// <summary>
        /// Conserva la posizione in cui è stato toccato lo schermo l'ultima volta.
        /// </summary>
        private Vector2 lastTouchPos;

        /// <summary>
        /// Contiene il numero di tocchi rilevati sullo schermo.
        /// </summary>
        private int touchCount = 0;

        /// <summary>
        /// Eseguito ad ogni fotogramma.
        /// </summary>
        private void Update()
        {
            //GlobalDialog.Instance.Debug(MainCameraPosition.ToString());
            if (enableInteractions)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        lastTouchTime = Time.time;
                        lastTouchPos = touch.position;
                        touchCount++;
                    }
                }

                if (touchCount > 0 && Time.time - lastTouchTime > touchLatence)
                {
                    OnTouch(touchCount, lastTouchPos);
                    touchCount = 0;
                }
            }
        }

        /// <summary>
        /// Eseguito quando un evento di tocco dello schermo è pronto ad essere gestito.
        /// </summary>
        private void OnTouch(int touchCount, Vector2 touchPos)
        {
            switch (touchCount)
            {
                case 1:
                    //Esegue un ar-raycast in base alla posizione del tocco sullo schermo.
                    List<ARRaycastHit> hits = new();
                    if (GetComponent<ARRaycastManager>().Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
                    {
                        //Aggiunge un marker alla posizione ottenuta dall'ar-raycast.
                        AddMarker(hits[0].pose.position);
                    }
                    break;
                case 2:
                    //Esegue un raycast in base alla posizione del tocco sullo schermo.
                    if (Physics.Raycast(mainCamera.ScreenPointToRay(touchPos), out RaycastHit hit, maxInteractionDistance, MARKER_LAYER_MASK))
                    {
                        //Rimuove il marker target e tutti quelli aggiunti in seguito.
                        ClearMarkersFrom(markerObjects.IndexOf(SphericalMarker.GetAnchor(hit.transform.gameObject)));

                        //Emette una vibrazione.
                        Handheld.Vibrate();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Abilita le interazioni con i marker.
        /// </summary>
        public void EnableInteractions()
        {
            enableInteractions = true;
            touchCount = 0;
        }

        /// <summary>
        /// Disabilita le interazioni con i marker.
        /// </summary>
        public void DisableInteractions()
        {
            enableInteractions = false;
            touchCount = 0;
        }

        /// <summary>
        /// Restituisce la posizione del marker nell'indice specificato.
        /// </summary>
        public Vector3 GetMarker(int index) => markerPositions[index];

        /// <summary>
        /// Aggiunge un marker nella posizione specificata.
        /// </summary>
        public void AddMarker(Vector3 markerPosition)
        {
            //Istanzia un nuovo gameobject per il marker.
            GameObject markerObject = Instantiate(markerPrefabs[0], markerPosition, Quaternion.identity);

            //Scosta il tempo così da avere animazioni progressive tra gli oggetti.
            SphericalMarker.GetMarkerComponent(markerObject).waveDelay = 0.2f * MarkerCount;

            //Memorizza il nuovo marker e la sua posizione.
            markerObjects.Add(markerObject);
            markerPositions.Add(markerObject.transform.position);
        }

        /// <summary>
        /// Cancella tutti i marker a partire da quello specificato in poi.
        /// </summary>
        public void ClearMarkersFrom(int index)
        {
            while (MarkerCount > index)
            {
                //Distrugge e rimuove il marker e la sua posizione.
                //I successivi saranno spostati indietro di 1 nella lista.
                Destroy(markerObjects[index]);
                markerObjects.RemoveAt(index);
                markerPositions.RemoveAt(index);
            }
        }

        /// <summary>
        /// Cancella tutti i marker.
        /// </summary>
        public void ClearMarkers()
        {
            //Distrugge e rimuove tutti i marker e le loro posizioni.
            markerObjects.ForEach(o => Destroy(o));
            markerObjects.Clear();
            markerPositions.Clear();
        }
    }
}
