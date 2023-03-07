using ARMaps.Core;
using ARMaps.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ARMaps
{
    /// <summary>
    /// Oggetto che si occupa di gestire la visualizzazione e la creazione di percorsi.
    /// </summary>
    [RequireComponent(typeof(MarkerManager))]
    public class PathManager : MonoBehaviour
    {
        /// <summary>
        /// Accede all'istanza del manager.
        /// </summary>
        public static PathManager Instance { get; private set; }

        [Tooltip("Pulsante cancella marker.")]
        [SerializeField] private Button clearButton;

        [Tooltip("Pulsante salva percorso.")]
        [SerializeField] private Button saveButton;

        [Tooltip("Pulsante annulla salvataggio percorso.")]
        [SerializeField] private Button discardButton;

        [Tooltip("Pulsante rimozione indicazioni.")]
        [SerializeField] private Button removeIndicationsButton;

        [Tooltip("Pulsante visualizzatore del pannello indicazioni.")]
        [SerializeField] private Button indicationsButton;

        [Tooltip("Distanza massima di piazzamento dei marker. Quelli già esistenti non saranno eliminati.")]
        [Range(0.1f, 50f)]
        public float loadingDistance = 5f;

        /// <summary>
        /// Gestore dei marker.
        /// </summary>
        private MarkerManager markerManager;

        /// <summary>
        /// Percorso attuale.
        /// </summary>
        private ARPath path = null;

        /// <summary>
        /// Indica il successivo marker da piazzare del percorso.
        /// </summary>
        private int nextMarkerToLoad = 0;

        /// <summary>
        /// Rappresenta la modalità attuale. (Visualizzazione indicazioni, creazione percorso...)
        /// </summary>
        private string mode = "none";

        /// <summary>
        /// Inizializza il path manager.
        /// </summary>
        private PathManager() { }

        /// <summary>
        /// Eseguito all'avvio.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                markerManager = GetComponent<MarkerManager>();
                clearButton.GetComponent<Button>().onClick.AddListener(OnClearClick);
                saveButton.GetComponent<Button>().onClick.AddListener(OnSaveClick);
                discardButton.GetComponent<Button>().onClick.AddListener(OnDiscardClick);
                removeIndicationsButton.GetComponent<Button>().onClick.AddListener(OnRemoveIndicationsClick);
            }
        }

        /// <summary>
        /// Eseguito ad ogni fotogramma.
        /// </summary>
        private void Update()
        {
            //Esegue un azione in base alla modalità specificata.
            switch (mode)
            {
                case "show_indications":
                    OnPlaceMarkers(path, markerManager.MainCameraPosition, loadingDistance);
                    break;
                case "create_path":
                    break;
                case "none":
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Piazza i markers del percorso specificato nella massima distanza di caricamento specificata.
        /// </summary>
        private void OnPlaceMarkers(ARPath path, Vector3 cameraPosition, float loadingDistance)
        {
            //Calcola la distanza tra la posizione attuale della telecamera e il marker da piazzare.
            while (nextMarkerToLoad < path.MarkerCount && Vector3.Distance(cameraPosition, path.GetMarker(nextMarkerToLoad)) < loadingDistance)
            {
                //Se la distanza calcolata è minore della massima distanza di caricamento, piazza il marker.
                markerManager.AddMarker(path.GetMarker(nextMarkerToLoad));
                nextMarkerToLoad++;
            }
        }

        /// <summary>
        /// Eseguito al click sul pulsante per cancellare tutti i marker presenti nel percorso.
        /// </summary>
        private void OnClearClick() => markerManager.ClearMarkers();

        /// <summary>
        /// Eseguito al click sul pulsante per salvare il percorso.
        /// </summary>
        private void OnSaveClick()
        {
            //Disattiva le interazioni con i marker.
            markerManager.DisableInteractions();

            //Memorizza nel percorso attuale le posizioni dei marker.
            path.ClearMarkers();
            for (int i = 0; i < markerManager.MarkerCount; i++)
            {
                path.AddMarker(markerManager.GetMarker(i));
            }

            //Rimuove tutti i marker piazzati nel mondo AR.
            markerManager.ClearMarkers();

            //Resetta i pulsanti.
            clearButton.gameObject.SetActive(false);
            saveButton.gameObject.SetActive(false);
            discardButton.gameObject.SetActive(false);
            indicationsButton.gameObject.SetActive(true);

            //Rimuove il riferimento al percorso in uso, e resetta la modalità su "nessuna".
            path = null;
            mode = "none";
        }

        /// <summary>
        /// Eseguito al click sul pulsante per annullare il salvataggio del percorso.
        /// </summary>
        private void OnDiscardClick()
        {
            //Disattiva le interazioni con i marker.
            markerManager.DisableInteractions();

            //Rimuove tutti i marker piazzati nel mondo AR.
            markerManager.ClearMarkers();

            //Resetta i pulsanti.
            clearButton.gameObject.SetActive(false);
            saveButton.gameObject.SetActive(false);
            discardButton.gameObject.SetActive(false);
            indicationsButton.gameObject.SetActive(true);

            //Rimuove il riferimento al percorso in uso, e resetta la modalità su "nessuna".
            path = null;
            mode = "none";
        }

        /// <summary>
        /// Eseguito al click sul pulsante di rimozione delle indicazioni attualmente mostrate.
        /// </summary>
        private void OnRemoveIndicationsClick()
        {
            //Rimuove tutti i marker piazzati nel mondo AR.
            markerManager.ClearMarkers();

            //Resetta i pulsanti.
            removeIndicationsButton.gameObject.SetActive(false);
            indicationsButton.gameObject.SetActive(true);

            //Rimuove il riferimento al percorso in uso, e resetta la modalità su "nessuna".
            //Resetta anche la posizione del successivo marker da caricare.
            path = null;
            mode = "none";
            nextMarkerToLoad = 0;
        }

        /// <summary>
        /// Visualizza il percorso specificato.
        /// </summary>
        public void ShowPathIndications(ARPath path)
        {
            if (path != null)
            {
                //Imposta il percorso.
                this.path = path;

                //Imposta i pulsanti per le indicazioni.
                removeIndicationsButton.gameObject.SetActive(true);
                indicationsButton.gameObject.SetActive(false);

                //Imposta la modalità di visualizzazione indicazioni.
                mode = "show_indications";
            }
            else
            {
                //Il percorso non può essere null, in caso contrario, sarà generata un eccezione.
                throw new ArgumentNullException(nameof(path));
            }
        }

        /// <summary>
        /// Avvia la modalità di creazione percorso.
        /// Se si clicca su salva, quindi, tutti i marker saranno salvati nel percorso specificato.
        /// Se il percorso non è vuoto, sarà svuotato, quindi saranno inseriti i nuovi marker.
        /// </summary>
        public void StartCreatingPath(ARPath path)
        {
            if (path != null)
            {
                //Imposta il percorso.
                this.path = path;

                //Attiva le interazioni con i marker.
                markerManager.DisableInteractions();

                //Imposta i pulsanti per la creazione percorso.
                clearButton.gameObject.SetActive(true);
                saveButton.gameObject.SetActive(true);
                discardButton.gameObject.SetActive(true);
                indicationsButton.gameObject.SetActive(false);

                //Imposta la modalità di creazione percorso.
                mode = "create_path";

                //Attiva le interazioni con i marker.
                markerManager.EnableInteractions();
            }
            else
            {
                //Il percorso non può essere null, in caso contrario, sarà generata un eccezione.
                throw new ArgumentNullException(nameof(path));
            }
        }
    }
}
