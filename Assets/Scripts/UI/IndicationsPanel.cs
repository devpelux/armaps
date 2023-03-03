using ARMaps.Core;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
//todo documentare tutto.
/// <summary>
/// Gestisce il pannello delle indicazioni.
/// </summary>
public class IndicationsPanel : OpenablePanel
{
    [Tooltip("Pulsante indicazioni.")]
    public GameObject indicationsButton;

    [Tooltip("Casella di testo che mostrerà il nome della mappa in uso.")]
    public GameObject mapName;

    [Tooltip("Pulsante mappe.")]
    public GameObject mapsButton;

    [Tooltip("Pulsante di chiusura.")]
    public GameObject closeButton;

    [Tooltip("Pannello ricerca sorgenti.")]
    public SearchPanel searchSource;

    [Tooltip("Pannello ricerca destinazioni.")]
    public SearchPanel searchDestination;

    [Tooltip("Pulsante ottieni indicazioni.")]
    public GameObject getIndications;

    [Tooltip("Pulsante crea percorso.")]
    public GameObject createPath;

    private string panelToOpen = "none";

    private string currentSource = "none";
    private string currentDestination = "none";

    private void Start()
    {
        mapsButton.GetComponent<Button>().onClick.AddListener(OnChangeMapClick);
        closeButton.GetComponent<Button>().onClick.AddListener(ClosePanel);
        getIndications.GetComponent<Button>().onClick.AddListener(OnGetIndicationsClick);
        createPath.GetComponent<Button>().onClick.AddListener(OnCreatePathClick);

        searchSource.OnSearchTextChange.AddListener(OnSearchSourceTextChanged);
        searchSource.OnResultClick.AddListener(OnSearchSourceResultClicked);
        searchDestination.OnSearchTextChange.AddListener(OnSearchDestinationTextChanged);
        searchDestination.OnResultClick.AddListener(OnSearchDestinationResultClicked);
    }

    private void OnSearchSourceTextChanged(string source)
    {
        searchSource.ClearResultButtons();
        if (source != "")
        {
            List<ARPath> paths = MapsManager.Instance.CurrentMap.FilterPaths(source);

            //Query sulle sorgenti, rimuove i duplicati e torna una lista di stringhe.
            IEnumerable<string> sources = (from ARPath path in paths select path.Source).Distinct();

            if (!sources.Contains(source))
            {
                searchSource.AddResultButton(source);
            }

            sources.Take(2).ToList().ForEach(s => searchSource.AddResultButton(s));
        }
    }

    private void OnSearchSourceResultClicked(string source)
    {
        if (currentSource != source)
        {
            currentSource = source;
            searchDestination.Clear();
            searchDestination.gameObject.SetActive(true);
            currentDestination = "none";
        }
        else
        {
            currentSource = "none";
            searchDestination.Clear();
            searchDestination.gameObject.SetActive(false);
            currentDestination = "none";
            getIndications.SetActive(false);
            createPath.SetActive(false);
        }
    }

    private void OnSearchDestinationTextChanged(string destination)
    {
        searchDestination.ClearResultButtons();
        if (destination != "")
        {
            List<ARPath> paths = MapsManager.Instance.CurrentMap.FilterPaths(currentSource, destination);

            //Query sulle destinazioni, rimuove i duplicati e torna una lista di stringhe.
            IEnumerable<string> destinations = (from ARPath path in paths select path.Destination).Distinct();

            if (!destinations.Contains(destination))
            {
                searchDestination.AddResultButton(destination);
            }

            destinations.Take(2).ToList().ForEach(s => searchDestination.AddResultButton(s));
        }
    }

    private void OnSearchDestinationResultClicked(string destination)
    {
        if (currentDestination != destination)
        {
            currentDestination = destination;

            if (MapsManager.Instance.CurrentMap.PathExists(currentSource, currentDestination))
            {
                getIndications.SetActive(true);
                createPath.SetActive(false);
            }
            else
            {
                getIndications.SetActive(false);
                createPath.SetActive(true);
            }
        }
        else
        {
            currentDestination = "none";
            getIndications.SetActive(false);
            createPath.SetActive(false);
        }
    }

    private void OnCreatePathClick()
    {
        panelToOpen = "createPath";
        ClosePanel();
    }

    private void OnGetIndicationsClick()
    {
        panelToOpen = "indications";
        ClosePanel();
    }

    private void OnChangeMapClick()
    {
        panelToOpen = "maps";
        ClosePanel();
    }

    public void ResetPanel()
    {
        mapName.GetComponent<MapName>().SetMapName(MapsManager.Instance.CurrentMap.Name);
        currentSource = "none";
        currentDestination = "none";
        searchSource.Clear();
        searchDestination.Clear();
        searchDestination.gameObject.SetActive(false);
        getIndications.SetActive(false);
        createPath.SetActive(false);
        panelToOpen = "none";
    }

    public override void OnBeforePanelOpening()
    {
        base.OnBeforePanelOpening();

        mapName.GetComponent<MapName>().SetMapName(MapsManager.Instance.CurrentMap.Name);

        ResetPanel();
    }

    public override void OnAfterPanelClosing()
    {
        base.OnAfterPanelClosing();

        if (panelToOpen == "maps")
        {
            PanelInstances.MapsPanel.OpenPanel();
        }
        else if (panelToOpen == "createPath")
        {
            PathCreationManager.Instance.StartCreatingPath(currentSource, currentDestination);
        }
        else if (panelToOpen == "indications")
        {
            indicationsButton.SetActive(true);
        }
        else if (panelToOpen == "none")
        {
            indicationsButton.SetActive(true);
        }

        ResetPanel();
    }
}
