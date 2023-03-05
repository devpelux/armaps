using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARMaps.Core
{
    /// <summary>
    /// Rappresenta un percorso.
    /// </summary>
    public class ARPath
    {
        /// <summary>
        /// Sorgente del percorso. (parte di chiave primaria)
        /// </summary>
        public string Source { get; }

        /// <summary>
        /// Destinazione del percorso. (parte di chiave primaria)
        /// </summary>
        public string Destination { get; }

        /// <summary>
        /// Numero di marcatori del percorso.
        /// </summary>
        public int MarkerCount => markers.Count;

        /// <summary>
        /// Lunghezza del percorso.
        /// </summary>
        public float PathLength => GetPathLength();

        /// <summary>
        /// Accede al marcatore nell'indice specificato.
        /// </summary>
        public Vector3 this[int index]
        {
            get => GetMarker(index);
            set => SetMarker(index, value);
        }

        /// <summary>
        /// Lista dei marcatori del percorso.
        /// </summary>
        private readonly List<Vector3> markers = new();

        /// <summary>
        /// Inizializza un percorso vuoto con sorgente e destinazione specificati.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public ARPath(string source, string destination)
        {
            if (source == null || source == "" || destination == null || destination == "")
            {
                throw new ArgumentException("Invalid path.");
            }
            Source = source;
            Destination = destination;
        }

        /// <summary>
        /// Aggiunge un marcatore.
        /// </summary>
        public void AddMarker(Vector3 pos) => markers.Add(pos);

        /// <summary>
        /// Restituisce il marcatore nell'indice specificato.
        /// </summary>
        public Vector3 GetMarker(int index) => markers[index];

        /// <summary>
        /// Sostituisce il marcatore nell'indice specificato con quello specificato.
        /// </summary>
        public void SetMarker(int index, Vector3 pos) => markers[index] = pos;

        /// <summary>
        /// Rimuove tutti i marcatori.
        /// </summary>
        public void ClearMarkers() => markers.Clear();

        /// <summary>
        /// Ottiene la lunghezza del percorso sommando le distanze dei markers.
        /// </summary>
        public float GetPathLength()
        {
            double length = 0;

            //Somma le distanze nel piano orizzontale (assi x e z).
            int count = MarkerCount - 1;
            for (int i = 0; i < count; i++)
            {
                //Ottiene la posizione corrente e la successiva.
                Vector3 curr = GetMarker(i);
                Vector3 succ = GetMarker(i + 1);

                //Calcola la distanza tra le 2 posizioni.
                float xDist = curr.x - succ.x;
                float yDist = curr.y - succ.y;
                length += Math.Sqrt(xDist * xDist + yDist * yDist);
            }

            return (float)length;
        }

        /// <summary>
        /// Confronta il percorso con un altro.
        /// Saranno uguali se hanno stessa sorgente e stessa destinazione.
        /// </summary>
        public override bool Equals(object obj) => obj is ARPath path && Matcher(path, Source, Destination);

        /// <summary>
        /// Restituisce l'hash code del percorso.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(Source.ToLower(), Destination.ToLower());

        /// <summary>
        /// Confronta un percorso specificato con la sorgente e la destinazione specificate.
        /// </summary>
        public static bool Matcher(ARPath path, string source, string destination)
            => path.Source.EqualsInsensitive(source) && path.Destination.EqualsInsensitive(destination);
    }
}
