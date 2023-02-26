using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestore dei percorsi memorizzati.
/// </summary>
public class PathsManager : MonoBehaviour
{
    /// <summary>
    /// Accede all'istanza del manager.
    /// </summary>
    public static PathsManager Instance { get; private set; }

    /// <summary>
    /// Lista dei percorsi memorizzati.
    /// </summary>
    private readonly List<Path> paths = new();

    private PathsManager() { }

    //Eseguito all'avvio.
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    /// <summary>
    /// Aggiunge un percorso.
    /// </summary>
    public void CreatePath(string name) => paths.Add(new Path(name));

    /// <summary>
    /// Rimuove il percorso con il nome specificato.
    /// </summary>
    public void RemovePath(Path path) => paths.Remove(path);

    /// <summary>
    /// Restituisce il percorso con il nome specificato o null se non esiste.
    /// </summary>
    public Path GetPath(string name) => paths.Find(p => p.Name == name);
}
