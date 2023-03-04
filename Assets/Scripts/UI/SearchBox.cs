using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ARMaps.UI
{
    /// <summary>
    /// Casella di ricerca.
    /// </summary>
    public class SearchBox : MonoBehaviour
    {
        [Tooltip("Label della casella di ricerca.")]
        [SerializeField] private TMP_Text label;

        [Tooltip("Casella di testo della casella di ricerca.")]
        [SerializeField] private TMP_InputField textBox;

        /// <summary>
        /// Eseguito all'avvio.
        /// </summary>
        private void Awake() => textBox.onValueChanged.AddListener(v => OnValueChanged.Invoke(v));

        /// <summary>
        /// Evento lanciato quando il testo è cambiato.
        /// Riceve come parametro il testo attuale.
        /// </summary>
        public UnityEvent<string> OnValueChanged { get; } = new();

        /// <summary>
        /// Evento lanciato quando la casella di ricerca è stata resettata.
        /// </summary>
        public UnityEvent OnReinitialized { get; } = new();

        /// <summary>
        /// Ottiene o imposta il testo.
        /// </summary>
        public string Text
        {
            get => textBox.text;
            set => textBox.text = value;
        }

        /// <summary>
        /// Ottiene o imposta la label.
        /// </summary>
        public string Label
        {
            get => label.text;
            set => label.text = value;
        }

        /// <summary>
        /// Ottiene o imposta la proprietà di sola lettura, che impedisce modifiche al testo.
        /// </summary>
        public bool ReadOnly
        {
            get => textBox.readOnly;
            set => textBox.readOnly = value;
        }

        /// <summary>
        /// Pulisce e resetta la casella di testo.
        /// </summary>
        public void Reinitialize()
        {
            textBox.readOnly = false;

            //Impedisce il lancio dell'evento OnValueChanged.
            //Al suo posto verrà lanciato OnReinitialized.
            textBox.SetTextWithoutNotify("");
            OnReinitialized.Invoke();
        }
    }
}
