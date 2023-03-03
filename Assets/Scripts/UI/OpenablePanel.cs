using UnityEngine;

/// <summary>
/// Gestisce un pannello "apri e chiudi" generico.
/// </summary>
[RequireComponent(typeof(Animator))]
public class OpenablePanel : MonoBehaviour
{
    /// <summary>
    /// Eseguito all'avvio. (il metodo accetta override)
    /// </summary>
    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Apre il pannello.
    /// </summary>
    public virtual void OpenPanel()
    {
        gameObject.SetActive(true);
        GetComponent<Animator>().SetBool("opened", true);
    }

    /// <summary>
    /// Chiude il pannello.
    /// </summary>
    public virtual void ClosePanel()
    {
        GetComponent<Animator>().SetBool("opened", false);
    }

    /// <summary>
    /// Eseguito prima che inizia l'animazione di apertura.
    /// </summary>
    public virtual void OnBeforePanelOpening() { }

    /// <summary>
    /// Eseguito dopo che è finita l'animazione di apertura.
    /// </summary>
    public virtual void OnAfterPanelOpening() { }

    /// <summary>
    /// Eseguito prima che inizia l'animazione di chiusura.
    /// </summary>
    public virtual void OnBeforePanelClosing() { }

    /// <summary>
    /// Eseguito dopo che è finita l'animazione di chiusura.
    /// </summary>
    public virtual void OnAfterPanelClosing()
    {
        gameObject.SetActive(false);
    }
}
