using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rappresenta un percorso.
/// </summary>
public class Path
{
    /// <summary>
    /// Nome del percorso.
    /// </summary>
    public string Name { get; set; } = "Generic";

    /// <summary>
    /// Nome dei marker del percorso.
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    /// Inizializza un percorso vuoto con il nome specificato.
    /// </summary>
    public Path(string name) => Name = name;

    /// <summary>
    /// Lista delle posizioni dei marcatori del percorso.
    /// </summary>
    private readonly List<Vector3> markers = new();

    /// <summary>
    /// Aggiunge un marcatore.
    /// </summary>
    public void AddMarker(Vector3 pos) => markers.Add(pos);

    /// <summary>
    /// Inserisce un marcatore nella posizione specificata spostando gli altri in avanti.
    /// </summary>
    public void InsertMarker(int index, Vector3 pos) => markers.Insert(index, pos);

    /// <summary>
    /// Restituisce il marcatore alla posizione specificata.
    /// </summary>
    public Vector3 GetMarker(int index) => markers[index];

    /// <summary>
    /// Rimuove il marcatore alla posizione specificata.
    /// </summary>
    public void RemoveMarker(int index) => markers.RemoveAt(index);

    /// <summary>
    /// Rimuove tutti i marcatori.
    /// </summary>
    public void ClearMarkers() => markers.Clear();

    /// <summary>
    /// Confronta il percorso con un altro.
    /// Saranno uguali se hanno lo stesso nome.
    /// </summary>
    public override bool Equals(object obj) => obj is Path path && Name == path.Name;

    /// <summary>
    /// Restituisce l'hash code del percorso.
    /// </summary>
    public override int GetHashCode() => Name.GetHashCode();
}
