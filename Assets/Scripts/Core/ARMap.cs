using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace ARMaps.Core
{
    /// <summary>
    /// Rappresenta una mappa.
    /// </summary>
    public class ARMap
    {
        /// <summary>
        /// Nome univoco della mappa. (chiave primaria)
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Numero di percorsi della mappa.
        /// </summary>
        public int PathCount => paths.Count;

        /// <summary>
        /// Lista dei percorsi della mappa.
        /// </summary>
        private readonly List<ARPath> paths = new();

        /// <summary>
        /// Inizializza una mappa vuota con il nome specificato.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public ARMap(string name)
        {
            if (name == null || name == "")
            {
                throw new ArgumentException("Invalid map.");
            }
            Name = name;
        }

        /// <summary>
        /// Crea e aggiunge un percorso.
        /// </summary>
        public ARPath CreatePath(string source, string destination)
        {
            ARPath path = new(source, destination);
            paths.Add(path);
            return path;
        }

        /// <summary>
        /// Restituisce il percorso nell'indice specificato.
        /// </summary>
        public ARPath GetPath(int index) => paths[index];

        /// <summary>
        /// Restituisce il percorso con sorgente e destinazione specificati.
        /// </summary>
        public ARPath GetPath(string source, string destination) => paths.Find(path => ARPath.Matcher(path, source, destination));

        /// <summary>
        /// Rimuove il percorso specificato.
        /// </summary>
        public void RemovePath(ARPath path) => paths.Remove(path);

        /// <summary>
        /// Restituisce tutti i percorsi che contengono nella sorgente il valore specificato.
        /// </summary>
        public List<ARPath> FilterPaths(string partialSource) => paths.FindAll(path => path.Source.ContainsInsensitive(partialSource));

        /// <summary>
        /// Restituisce tutti i percorsi con la sorgente specificata, e che contengono nella destinazione il valore specificato.
        /// </summary>
        public List<ARPath> FilterPathsOfSource(string source, string partialDestination)
            => paths.FindAll(path => path.Source.EqualsInsensitive(source) && path.Destination.ContainsInsensitive(partialDestination));

        /// <summary>
        /// Confronta la mappa con un altra.
        /// Saranno uguali se hanno lo stesso nome.
        /// </summary>
        public override bool Equals(object obj) => obj is ARMap map && Matcher(map, Name);

        /// <summary>
        /// Restituisce l'hash code della mappa.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(Name.ToLower());

        /// <summary>
        /// Confronta una mappa specificata con il nome specificato.
        /// </summary>
        public static bool Matcher(ARMap map, string name) => map.Name.EqualsInsensitive(name);
    }
}
