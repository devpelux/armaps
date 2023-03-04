using UnityEngine;

namespace ARMaps.UI
{
    /// <summary>
    /// Pannello "apri e chiudi" generico.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class OpenablePanel : MonoBehaviour
    {
        /// <summary>
        /// Eseguito all'avvio. (Il metodo accetta override)
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
            //Attiva il pannello e lo apre.
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
        /// Resetta il pannello.
        /// </summary>
        public virtual void ResetPanel() { }

        /// <summary>
        /// Eseguito prima che inizia l'animazione di apertura.
        /// </summary>
        public virtual void OnBeforePanelOpening()
        {
            //Resetta il pannello.
            ResetPanel();
        }

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
            //Resetta il pannello e lo disattiva.
            ResetPanel();
            gameObject.SetActive(false);
        }
    }
}
