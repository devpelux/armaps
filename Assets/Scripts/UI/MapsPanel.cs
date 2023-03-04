using ARMaps.Core;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ARMaps.UI
{
    /// <summary>
    /// Gestisce il pannello delle mappe.
    /// </summary>
    public class MapsPanel : OpenablePanel
    {
        [Tooltip("Casella di ricerca.")]
        [SerializeField] private TMP_InputField searchBox;

        [Tooltip("Lista contenente i risultati di ricerca selezionabile.")]
        [SerializeField] private ButtonList searchResultsButtonList;

        [Tooltip("Pulsante creazione mappa.")]
        [SerializeField] private Button createMapButton;

        [Tooltip("Pulsante indietro.")]
        [SerializeField] private Button backButton;

        /// <summary>
        /// Eseguito all'avvio. (Il metodo accetta override)
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            //Registra i listener degli eventi delle componenti.
            searchBox.onValueChanged.AddListener(OnSearchBoxTextChanged);
            searchResultsButtonList.OnButtonClick.AddListener(OnSearchResultClicked);
            createMapButton.onClick.AddListener(OnCreateMapButtonClick);
            backButton.onClick.AddListener(ClosePanel);
        }

        /// <summary>
        /// Resetta il pannello.
        /// </summary>
        public override void ResetPanel()
        {
            //Ripulisce la casella di ricerca.
            //Rimuove tutti i pulsanti dei risultati.
            //Disattiva il pulsante di creazione mappa.
            searchBox.text = "";
            searchResultsButtonList.RemoveAllButtons();
            createMapButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// Eseguito dopo che è finita l'animazione di chiusura.
        /// </summary>
        public override void OnAfterPanelClosing()
        {
            //Apre il pannello delle indicazioni.
            PanelInstances.IndicationsPanel.OpenPanel();

            base.OnAfterPanelClosing();
        }

        /// <summary>
        /// Eseguito quando la casella di ricerca cambia il valore del testo.
        /// </summary>
        private void OnSearchBoxTextChanged(string mapName)
        {
            //Rimuove i risultati di un eventuale ricerca precedente.
            searchResultsButtonList.RemoveAllButtons();

            //Filtra le mappe ottenendo quelle che contengono il nome cercato.
            List<ARMap> maps = MapsManager.Instance.FilterMaps(mapName);

            //Query sui nomi delle mappe, rimuove i duplicati e torna una lista di stringhe.
            IEnumerable<string> mapNames = (from ARMap map in maps select map.Name).Distinct();

            //Se la mappa non esiste, verrà mostrato il pulsante di creazione mappa.
            createMapButton.gameObject.SetActive(mapName != "" && !mapNames.Contains(mapName));

            //Vengono quindi mostrati i primi 3 risultati di ricerca.
            mapNames.Take(3).ToList().ForEach(m => searchResultsButtonList.AddButton(m));
        }

        /// <summary>
        /// Eseguito al click su un pulsante dei risultati di ricerca.
        /// </summary>
        private void OnSearchResultClicked(string mapName)
        {
            //Cambia la mappa corrente con quella scelta (che esisterà).
            //Chiude quindi il pannello.
            MapsManager.Instance.SwitchMap(mapName);
            ClosePanel();
        }

        /// <summary>
        /// Eseguito al click sul pulsante di creazione mappa.
        /// </summary>
        private void OnCreateMapButtonClick()
        {
            //Cambia la mappa corrente con quella scelta (se non esiste, verrà creata).
            //Chiude quindi il pannello.
            MapsManager.Instance.SwitchMap(searchBox.text);
            ClosePanel();
        }
    }
}
