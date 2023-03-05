using System.Collections.Generic;
using Unity.VisualScripting;
using static Unity.VisualScripting.Member;

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
        public const string DEFAULT_MAP = "Default";

        /// <summary>
        /// Mappa attualmente in uso.
        /// </summary>
        public ARMap CurrentMap { get; private set; }

        /// <summary>
        /// Lista delle mappe memorizzate.
        /// </summary>
        private readonly List<ARMap> maps = new();

        /// <summary>
        /// Inizializza il gestore delle mappe.
        /// </summary>
        private MapsManager() => SwitchMap(DEFAULT_MAP);

        /// <summary>
        /// Sostituisce la mappa corrente con quella specificata.
        /// Crea la mappa se non esiste.
        /// </summary>
        public void SwitchMap(string name) => CurrentMap = GetMap(name) ?? CreateMap(name);

        /// <summary>
        /// Crea e aggiunge una mappa.
        /// </summary>
        public ARMap CreateMap(string name)
        {
            ARMap map = new(name);
            maps.Add(map);
            return map;
        }

        /// <summary>
        /// Restituisce la mappa nell'indice specificato.
        /// </summary>
        public ARMap GetMap(int index) => maps[index];

        /// <summary>
        /// Restituisce la mappa con il nome specificato.
        /// </summary>
        public ARMap GetMap(string name) => maps.Find(map => ARMap.Matcher(map, name));

        /// <summary>
        /// Rimuove la mappa specificata.
        /// Non è possibile rimuovere la mappa predefinita.
        /// </summary>
        public void RemoveMap(ARMap map)
        {
            if (!map.Equals(DEFAULT_MAP))
            {
                maps.Remove(map);
            }
        }

        /// <summary>
        /// Restituisce una lista di mappe che contengono nel nome il valore specificato.
        /// </summary>
        public List<ARMap> FilterMaps(string partialName) => maps.FindAll(map => map.Name.ContainsInsensitive(partialName));
    }
}
