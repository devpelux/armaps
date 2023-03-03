using ARMaps.Core;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Gestore delle mappe memorizzate.
/// </summary>
public class MapsManager : MonoBehaviour
{
    public const string DEFAULT_MAP = "Mappa Predefinita";

    /// <summary>
    /// Accede all'istanza del manager.
    /// </summary>
    public static MapsManager Instance { get; private set; }

    /// <summary>
    /// Mappa attualmente in uso.
    /// </summary>
    public ARMap CurrentMap { get; private set; }

    /// <summary>
    /// Lista delle mappe memorizzate.
    /// </summary>
    private readonly List<ARMap> maps = new();

    private MapsManager() { }

    /// <summary>
    /// Eseguito all'avvio.
    /// </summary>
    private void Awake()
    {
        Instance = this;
        SwitchMap(DEFAULT_MAP);
    }

    /// <summary>
    /// Sostituisce la mappa corrente con quella specificata.
    /// Crea la mappa se non esiste.
    /// </summary>
    public void SwitchMap(string name)
    {
        ARMap map = maps.Find(map => map.Name == name);
        if (map == null)
        {
            map = new(name);
            maps.Add(map);
        }

        CurrentMap = map;
    }

    /// <summary>
    /// Rimuove la mappa corrente.
    /// </summary>
    //public void RemoveMap() => maps.Remove(CurrentMap);

    /// <summary>
    /// Restituisce una lista di mappe che contengono nel nome il valore specificato.
    /// </summary>
    public List<ARMap> FilterMaps(string partialName) => maps.FindAll(map => map.Name.ContainsInsensitive(partialName));

    /// <summary>
    /// Verifica se una mappa esiste.
    /// </summary>
    public bool Exists(string name) => maps.Exists(map => map.Name == name);
}
