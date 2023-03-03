using ARMaps.Core;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gestisce il pannello delle mappe.
/// </summary>
public class MapsPanel : OpenablePanel
{
    [Tooltip("Pannello ricerca mappe.")]
    public SearchPanel searchMap;

    [Tooltip("Pulsante creazione mappa.")]
    public GameObject createMapButton;

    [Tooltip("Pulsante indietro.")]
    public GameObject backButton;

    private void Start()
    {
        searchMap.OnSearchTextChange.AddListener(OnSearchMapTextChanged);
        searchMap.OnResultClick.AddListener(OnSearchMapResultClicked);
        createMapButton.GetComponent<Button>().onClick.AddListener(OnCreateMapButtonClick);
        backButton.GetComponent<Button>().onClick.AddListener(ClosePanel);
    }

    public override void OnBeforePanelOpening()
    {
        base.OnBeforePanelOpening();

        ResetPanel();
    }

    public override void OnAfterPanelClosing()
    {
        base.OnAfterPanelClosing();

        PanelInstances.IndicationsPanel.OpenPanel();

        ResetPanel();
    }

    public void ResetPanel()
    {
        searchMap.Clear();
        createMapButton.SetActive(false);
    }

    public void OnCreateMapButtonClick()
    {
        MapsManager.Instance.SwitchMap(searchMap.Text);
        ClosePanel();
    }

    private void OnSearchMapResultClicked(string mapName)
    {
        MapsManager.Instance.SwitchMap(mapName);
        ClosePanel();
    }

    private void OnSearchMapTextChanged(string mapName)
    {
        searchMap.ClearResultButtons();

        List<ARMap> maps = MapsManager.Instance.FilterMaps(mapName);

        //Query sui nomi delle mappe, rimuove i duplicati e torna una lista di stringhe.
        IEnumerable<string> mapNames = (from ARMap map in maps select map.Name).Distinct();

        createMapButton.SetActive(mapName != "" && !mapNames.Contains(mapName));

        mapNames.Take(3).ToList().ForEach(m => searchMap.AddResultButton(m));
    }
}
