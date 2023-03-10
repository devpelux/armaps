using ARMaps.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;

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

        [Tooltip("Pulsante cancella percorso.")]
        [SerializeField] private Button deletePathButton;

        [Tooltip("Pulsante mappe.")]
        [SerializeField] private Button mapsButton;

        [Tooltip("Pulsante di chiusura.")]
        [SerializeField] private Button closeButton;

        [Tooltip("Casella di ricerca sorgenti.")]
        [SerializeField] private SearchBox searchSourceBox;

        [Tooltip("Lista contenente i risultati della ricerca delle sorgenti selezionabile.")]
        [SerializeField] private ButtonList searchSourceResultsButtonList;

        [Tooltip("Casella di ricerca destinazioni.")]
        [SerializeField] private SearchBox searchDestinationBox;

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
        /// Percorso corrente.
        /// </summary>
        private ARPath currentPath = null;

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
            deletePathButton.onClick.AddListener(OnDeletePathButtonClick);
            mapsButton.onClick.AddListener(OnMapsButtonClick);
            closeButton.onClick.AddListener(ClosePanel);

            //Registra i listener degli eventi di ricerca e selezione.
            searchSourceBox.OnValueChanged.AddListener(OnSearchSourceBoxTextChanged);
            searchSourceBox.OnReinitialized.AddListener(() => OnSearchSourceBoxTextChanged(""));
            searchDestinationBox.OnValueChanged.AddListener(OnSearchDestinationBoxTextChanged);
            searchDestinationBox.OnReinitialized.AddListener(() => OnSearchDestinationBoxTextChanged(""));
            searchSourceResultsButtonList.OnButtonClick.AddListener(OnSearchSourceResultClicked);
            searchDestinationResultsButtonList.OnButtonClick.AddListener(OnSearchDestinationResultClicked);
        }

        /// <summary>
        /// Resetta il pannello.
        /// </summary>
        public override void Reinitialize()
        {
            //Imposta la mappa corrente nel titolo.
            titleText.text = preTitle + " " + MapsManager.Instance.CurrentMap.Name;

            //Rimuove le selezioni.
            selectedSource = selectedDestination = null;
            currentPath = null;

            //Resetta le caselle di ricerca.
            searchSourceBox.Reinitialize();
            searchDestinationBox.Reinitialize();

            //Disattiva la casella di ricerca delle destinazioni.
            searchDestinationBox.gameObject.SetActive(false);
            searchDestinationResultsButtonList.gameObject.SetActive(false);

            //Disattiva tutti i pulsanti eventualmente attivati.
            getIndicationsButton.gameObject.SetActive(false);
            createPathButton.gameObject.SetActive(false);
            deletePathButton.gameObject.SetActive(false);

            //Rimiove l'azione alla chiusura eventualmente impostata.
            actionOnClose = "none";
        }

        /// <summary>
        /// Eseguito dopo che ? finita l'animazione di chiusura.
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
                    PathManager.Instance.StartCreatingPath(MapsManager.Instance.CurrentMap.CreatePath(selectedSource, selectedDestination));
                    break;
                case "get_indications":
                    PathManager.Instance.ShowPathIndications(currentPath);
                    break;
                case "none":
                    indicationsButton.gameObject.SetActive(true);
                    break;
                default:
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
            if (source != "" && !sources.Contains(source, StringComparer.OrdinalIgnoreCase))
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

            //Se la sorgente ? null non ? possibile cercare una destinazione raggiungibile.
            if (selectedSource != null)
            {
                //Filtra i percorsi ottenendo quelli che contengono nella destinazione il valore cercato.
                List<ARPath> paths = MapsManager.Instance.CurrentMap.FilterPathsOfSource(selectedSource, destination);

                //Query sulle destinazioni, rimuove i duplicati e torna una lista di stringhe.
                IEnumerable<string> destinations = (from ARPath path in paths select path.Destination).Distinct();

                //Aggiunge un pulsante con il testo cercato, se non esiste, e visualizza 2 risultati.
                //Altrimenti visualizza 3 risultati.
                if (destination != "" && !destinations.Contains(destination, StringComparer.OrdinalIgnoreCase))
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
                //Se la sorgente selezionata ? null, allora non ? ancora stato selezionato nulla.
                //Procede alla selezione.
                selectedSource = source;

                //Imposta la casella di ricerca come read-only quindi attiva la ricerca della destinazione.
                searchSourceBox.ReadOnly = true;
                searchDestinationResultsButtonList.gameObject.SetActive(true);
                searchDestinationBox.gameObject.SetActive(true);
                searchDestinationBox.Reinitialize();
            }
            else
            {
                //Se la sorgente selezionata ? non null, allora ? stato selezionato qualcosa.
                //Procede alla deselezione di sorgente e destinazione (senza sorgente non si pu? definire una destinazione).
                selectedSource = selectedDestination = null;
                currentPath = null;

                //Imposta la casella di ricerca come non read-only.
                searchSourceBox.ReadOnly = false;

                //Resetta e disattiva la ricerca della destinazione.
                searchDestinationBox.Reinitialize();
                searchDestinationBox.gameObject.SetActive(false);
                searchDestinationResultsButtonList.gameObject.SetActive(false);

                //Disattiva tutti i pulsanti eventualmente attivati.
                getIndicationsButton.gameObject.SetActive(false);
                createPathButton.gameObject.SetActive(false);
                deletePathButton.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Eseguito al click su un pulsante dei risultati di ricerca delle destinazioni.
        /// </summary>
        private void OnSearchDestinationResultClicked(string destination)
        {
            if (selectedDestination == null)
            {
                //Se la destinazione selezionata ? null, allora non ? ancora stato selezionato nulla.
                //Procede alla selezione.
                selectedDestination = destination;

                //Imposta la casella di ricerca come read-only.
                searchDestinationBox.ReadOnly = true;

                //Attiva il pulsante corretto tra "crea percorso" e "ottieni indicazioni".
                if ((currentPath = MapsManager.Instance.CurrentMap.GetPath(selectedSource, selectedDestination)) != null)
                {
                    //Se il percorso con sorgente e destinazione selezionate esiste, attiva il pulsante "ottieni indicazioni".
                    getIndicationsButton.gameObject.SetActive(true);
                    createPathButton.gameObject.SetActive(false);
                    deletePathButton.gameObject.SetActive(true);
                }
                else
                {
                    //Se il percorso con sorgente e destinazione selezionate non esiste, attiva il pulsante "crea percorso".
                    getIndicationsButton.gameObject.SetActive(false);
                    createPathButton.gameObject.SetActive(true);
                    deletePathButton.gameObject.SetActive(false);
                }
            }
            else
            {
                //Se la sorgente selezionata ? non null, allora ? stato selezionato qualcosa.
                //Procede alla deselezione.
                selectedDestination = null;
                currentPath = null;

                //Imposta la casella di ricerca come non read-only, e disattiva il pulsante eventualmente attivato prima.
                searchDestinationBox.ReadOnly = false;
                getIndicationsButton.gameObject.SetActive(false);
                createPathButton.gameObject.SetActive(false);
                deletePathButton.gameObject.SetActive(false);
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
        /// Eseguito al click sul pulsante di creazione percorso.
        /// </summary>
        private void OnDeletePathButtonClick()
        {
            //Chiede se cancellare il percorso.
            GlobalDialog.Instance.Call("Confermi?", "Il percorso verr? cancellato definitivamente e non sar? possibile recuperarlo!",
                result =>
                {
                    if (result == GlobalDialog.Result.YES)
                    {
                        //Se ? stata confermata la cancellazione cancella il percorso.
                        MapsManager.Instance.CurrentMap.RemovePath(currentPath);

                        //Resetta il pannello per una nuova ricerca.
                        Reinitialize();
                    }
                });
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
