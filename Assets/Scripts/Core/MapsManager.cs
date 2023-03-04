using System.Collections.Generic;
using Unity.VisualScripting;

namespace ARMaps.Core
{
    /// <summary>
    /// Gestore delle mappe memorizzate.
    /// </summary>
    public class MapsManager
    {
        /// <summary>
        /// Accede all'istanza del manager.
        /// </summary>
        public static MapsManager Instance { get; } = new();

        /// <summary>
        /// Mappa predefinita.
        /// </summary>
        public const string DEFAULT_MAP = "Generic";

        /// <summary>
        /// Mappa attualmente in uso.
        /// </summary>
        public ARMap CurrentMap { get; private set; }

        /// <summary>
        /// Restituisce la mappa nell'indice specificato.
        /// </summary>
        public ARMap this[int index] => GetMap(index);

        /// <summary>
        /// Lista delle mappe memorizzate.
        /// </summary>
        private readonly List<ARMap> maps = new();

        /// <summary>
        /// Inizializza un gestore delle mappe.
        /// </summary>
        private MapsManager() => SwitchMap(DEFAULT_MAP);

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
        /// Restituisce la mappa nell'indice specificato.
        /// </summary>
        public ARMap GetMap(int index) => maps[index];

        /// <summary>
        /// Rimuove la mappa corrente.
        /// Non è possibile rimuovere la mappa predefinita.
        /// </summary>
        public void RemoveMap()
        {
            if (CurrentMap.Name != DEFAULT_MAP)
            {
                maps.Remove(CurrentMap);
            }
        }

        /// <summary>
        /// Restituisce una lista di mappe che contengono nel nome il valore specificato.
        /// </summary>
        public List<ARMap> FilterMaps(string partialName) => maps.FindAll(map => map.Name.ContainsInsensitive(partialName));

        /// <summary>
        /// Verifica se una mappa esiste.
        /// </summary>
        public bool Exists(string name) => maps.Exists(map => map.Name == name);
    }
}
