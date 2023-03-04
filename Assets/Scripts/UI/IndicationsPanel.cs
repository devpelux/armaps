using ARMaps.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

namespace ARMaps.UI
{
    /// <summary>
    /// Gestisce il pannello delle indicazioni.
    /// </summary>
    public class IndicationsPanel : OpenablePanel
    {
        [Tooltip("Casella di testo del titolo.")]
        [SerializeField] private TMP_Text titleText;

        [Tooltip("Pulsante indicazioni.")]
        [SerializeField] private Button indicationsButton;

        [Tooltip("Pulsante ottieni indicazioni.")]
        [SerializeField] private Button getIndicationsButton;

        [Tooltip("Pulsante crea percorso.")]
        [SerializeField] private Button createPathButton;

        [Tooltip("Pulsante mappe.")]
        [SerializeField] private Button mapsButton;

        [Tooltip("Pulsante di chiusura.")]
        [SerializeField] private Button closeButton;

        [Tooltip("Casella di ricerca sorgenti.")]
        [SerializeField] private TMP_InputField searchSourceBox;

        [Tooltip("Lista contenente i risultati della ricerca delle sorgenti selezionabile.")]
        [SerializeField] private ButtonList searchSourceResultsButtonList;

        [Tooltip("Casella di ricerca destinazioni.")]
        [SerializeField] private TMP_InputField searchDestinationBox;

        [Tooltip("Lista contenente i risultati della ricerca delle sorgenti selezionabile.")]
        [SerializeField] private ButtonList searchDestinationResultsButtonList;

        [Tooltip("Parte iniziale del titolo prima del nome della mappa in uso.")]
        public string preTitle;

        /// <summary>
        /// Sorgente attualmente selezionata.
        /// </summary>
        private string selectedSource = null;

        /// <summary>
        /// Destinazione attualmente selezionata.
        /// </summary>
        private string selectedDestination = null;

        /// <summary>
        /// Azione da eseguore alla chiusura del pannello.
        /// </summary>
        private string actionOnClose = "none";

        /// <summary>
        /// Eseguito all'avvio. (Il metodo accetta override)
        /// </summary>
        protected override void Awake()
        {
            //Registra i click listener dei pulsanti.
            getIndicationsButton.onClick.AddListener(OnGetIndicationsButtonClick);
            createPathButton.onClick.AddListener(OnCreatePathButtonClick);
            mapsButton.onClick.AddListener(OnMapsButtonClick);
            closeButton.onClick.AddListener(ClosePanel);

            //Registra i listener degli eventi di ricerca e selezione.
            searchSourceBox.onValueChanged.AddListener(OnSearchSourceBoxTextChanged);
            searchDestinationBox.onValueChanged.AddListener(OnSearchDestinationBoxTextChanged);
            searchSourceResultsButtonList.OnButtonClick.AddListener(OnSearchSourceResultClicked);
            searchDestinationResultsButtonList.OnButtonClick.AddListener(OnSearchDestinationResultClicked);
        }

        /// <summary>
        /// Resetta il pannello.
        /// </summary>
        public override void ResetPanel()
        {
            //Imposta la mappa corrente nel titolo.
            titleText.text = preTitle + " " + MapsManager.Instance.CurrentMap.Name;

            //Rimuove le selezioni.
            selectedSource = selectedDestination = null;

            //Resetta le caselle di ricerca.
            searchSourceResultsButtonList.RemoveAllButtons();
            searchSourceBox.text = "";
            searchSourceBox.readOnly = false;
            searchDestinationResultsButtonList.RemoveAllButtons();
            searchDestinationBox.text = "";
            searchDestinationBox.readOnly = false;

            //Disattiva la casella di ricerca delle destinazioni.
            searchDestinationBox.transform.parent.gameObject.SetActive(false);
            searchDestinationResultsButtonList.gameObject.SetActive(false);

            //Disattiva tutti i pulsanti eventualmente attivati.
            getIndicationsButton.gameObject.SetActive(false);
            createPathButton.gameObject.SetActive(false);

            //Rimiove l'azione alla chiusura eventualmente impostata.
            actionOnClose = "none";
        }

        /// <summary>
        /// Eseguito dopo che è finita l'animazione di chiusura.
        /// </summary>
        public override void OnAfterPanelClosing()
        {
            //Esegue l'azione che deve essere eseguita alla chiusura specificata.
            switch (actionOnClose)
            {
                case "show_maps":
                    PanelInstances.MapsPanel.OpenPanel();
                    break;
                case "create_path":
                    PathCreationManager.Instance.StartCreatingPath(selectedSource, selectedDestination);
                    break;
                case "get_indications":
                    indicationsButton.gameObject.SetActive(true);
                    break;
                case "none":
                    indicationsButton.gameObject.SetActive(true);
                    break;
            }

            base.OnAfterPanelClosing();
        }

        /// <summary>
        /// Eseguito quando la casella di ricerca delle sorgenti cambia il valore del testo.
        /// </summary>
        private void OnSearchSourceBoxTextChanged(string source)
        {
            //Rimuove i risultati di un eventuale ricerca precedente.
            searchSourceResultsButtonList.RemoveAllButtons();

            //Filtra i percorsi ottenendo quelli che contengono nella sorgente il valore cercato.
            List<ARPath> paths = MapsManager.Instance.CurrentMap.FilterPaths(source);

            //Query sulle sorgenti, rimuove i duplicati e torna una lista di stringhe.
            IEnumerable<string> sources = (from ARPath path in paths select path.Source).Distinct();

            //Aggiunge un pulsante con il testo cercato, se non esiste, e visualizza 2 risultati.
            //Altrimenti visualizza 3 risultati.
            if (source != "" && !sources.Contains(source))
            {
                searchSourceResultsButtonList.AddButton(source);
                sources.Take(2).ToList().ForEach(s => searchSourceResultsButtonList.AddButton(s));
            }
            else
            {
                sources.Take(3).ToList().ForEach(s => searchSourceResultsButtonList.AddButton(s));
            }
        }

        /// <summary>
        /// Eseguito quando la casella di ricerca delle destinazioni cambia il valore del testo.
        /// </summary>
        private void OnSearchDestinationBoxTextChanged(string destination)
        {
            //Rimuove i risultati di un eventuale ricerca precedente.
            searchDestinationResultsButtonList.RemoveAllButtons();

            //Se la sorgente è null non è possibile cercare una destinazione raggiungibile.
            if (selectedSource != null)
            {
                //Filtra i percorsi ottenendo quelli che contengono nella destinazione il valore cercato.
                List<ARPath> paths = MapsManager.Instance.CurrentMap.FilterPaths(selectedSource, destination);

                //Query sulle destinazioni, rimuove i duplicati e torna una lista di stringhe.
                IEnumerable<string> destinations = (from ARPath path in paths select path.Destination).Distinct();

                //Aggiunge un pulsante con il testo cercato, se non esiste, e visualizza 2 risultati.
                //Altrimenti visualizza 3 risultati.
                if (destination != "" && !destinations.Contains(destination))
                {
                    searchDestinationResultsButtonList.AddButton(destination);
                    destinations.Take(2).ToList().ForEach(s => searchDestinationResultsButtonList.AddButton(s));
                }
                else
                {
                    destinations.Take(3).ToList().ForEach(s => searchDestinationResultsButtonList.AddButton(s));
                }
            }
        }

        /// <summary>
        /// Eseguito al click su un pulsante dei risultati di ricerca delle sorgenti.
        /// </summary>
        private void OnSearchSourceResultClicked(string source)
        {
            if (selectedSource == null)
            {
                //Se la sorgente selezionata è null, allora non è ancora stato selezionato nulla.
                //Procede alla selezione.
                selectedSource = source;

                //Imposta la casella di ricerca come read-only quindi attiva la ricerca della destinazione.
                searchSourceBox.readOnly = true;
                searchDestinationBox.transform.parent.gameObject.SetActive(true);
                searchDestinationResultsButtonList.gameObject.SetActive(true);
            }
            else
            {
                //Se la sorgente selezionata è non null, allora è stato selezionato qualcosa.
                //Procede alla deselezione di sorgente e destinazione (senza sorgente non si può definire una destinazione).
                selectedSource = selectedDestination = null;

                //Imposta la casella di ricerca come non read-only.
                searchSourceBox.readOnly = true;

                //Resetta e disattiva la ricerca della destinazione.
                searchDestinationResultsButtonList.RemoveAllButtons();
                searchDestinationBox.readOnly = false;
                searchDestinationBox.text = "";
                searchDestinationBox.transform.parent.gameObject.SetActive(false);
                searchDestinationResultsButtonList.gameObject.SetActive(false);

                //Disattiva tutti i pulsanti eventualmente attivati.
                getIndicationsButton.gameObject.SetActive(false);
                createPathButton.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Eseguito al click su un pulsante dei risultati di ricerca delle destinazioni.
        /// </summary>
        private void OnSearchDestinationResultClicked(string destination)
        {
            if (selectedDestination == null)
            {
                //Se la destinazione selezionata è null, allora non è ancora stato selezionato nulla.
                //Procede alla selezione.
                selectedDestination = destination;

                //Imposta la casella di ricerca come read-only.
                searchDestinationBox.readOnly = true;

                //Attiva il pulsante corretto tra "crea percorso" e "ottieni indicazioni".
                if (MapsManager.Instance.CurrentMap.PathExists(selectedSource, selectedDestination))
                {
                    //Se il percorso con sorgente e destinazione selezionate esiste, attiva il pulsante "ottieni indicazioni".
                    getIndicationsButton.gameObject.SetActive(true);
                    createPathButton.gameObject.SetActive(false);
                }
                else
                {
                    //Se il percorso con sorgente e destinazione selezionate non esiste, attiva il pulsante "crea percorso".
                    getIndicationsButton.gameObject.SetActive(false);
                    createPathButton.gameObject.SetActive(true);
                }
            }
            else
            {
                //Se la sorgente selezionata è non null, allora è stato selezionato qualcosa.
                //Procede alla deselezione.
                selectedDestination = null;

                //Imposta la casella di ricerca come non read-only, e disattiva il pulsante eventualmente attivato prima.
                searchSourceBox.readOnly = true;
                getIndicationsButton.gameObject.SetActive(false);
                createPathButton.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Eseguito al click sul pulsante di creazione percorso.
        /// </summary>
        private void OnCreatePathButtonClick()
        {
            actionOnClose = "create_path";
            ClosePanel();
        }

        /// <summary>
        /// Eseguito al click sul pulsante per ottenere le indicazioni del percorso.
        /// </summary>
        private void OnGetIndicationsButtonClick()
        {
            actionOnClose = "get_indications";
            ClosePanel();
        }

        /// <summary>
        /// Eseguito al click sul pulsante delle mappe.
        /// </summary>
        private void OnMapsButtonClick()
        {
            actionOnClose = "show_maps";
            ClosePanel();
        }
    }
}
