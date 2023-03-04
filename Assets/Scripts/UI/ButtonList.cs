using ARMaps.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ARMaps.UI
{
    /// <summary>
    /// Gestisce una lista di pulsanti.
    /// </summary>
    public class ButtonList : MonoBehaviour
    {
        [Tooltip("Prefab dei pulsanti.")]
        [SerializeField] private Button buttonPrefab;

        [Tooltip("Indica se è possibile selezionare un pulsante, nascondendo gli altri.")]
        public bool selectable;

        [Header("Selected Button Properties")]

        [Tooltip("Colori del pulsante selezionato.")]
        public ColorBlock selectedButtonColors;

        /// <summary>
        /// Evento lanciato al click su un pulsante.
        /// Riceve come parametro il testo del pulsante.
        /// </summary>
        public UnityEvent<string> OnButtonClick { get; } = new();

        /// <summary>
        /// Lista dei pulsanti.
        /// </summary>
        private readonly List<GameObject> buttonList = new();

        /// <summary>
        /// Posizione originale del pulsante selezionato, o null se non è selezionato nessun pulsante.
        /// </summary>
        private Vector2? originalSelectedBtnPos = null;

        /// <summary>
        /// Colore originale del pulsante selezionato, o null se non è selezionato nessun pulsante.
        /// </summary>
        private ColorBlock? originalSelectedBtnColors = null;

        /// <summary>
        /// Aggiunge un pulsante con il testo specificato.
        /// </summary>
        public void AddButton(string text)
        {
            //Istanzia un nuovo pulsante, quindi gli inserisce il testo spcificato e registra il click listener.
            GameObject button = Instantiate(buttonPrefab.gameObject, transform);
            button.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
            button.GetComponent<Button>().onClick.AddListener(() => OnButtonClicked(button, text));

            //Il pulsante verrà posizionato immediatamente sotto gli altri aggiunti prima.
            button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20 * buttonList.Count);
            buttonList.Add(button);
        }

        /// <summary>
        /// Rimuove tutti i pulsanti.
        /// </summary>
        public void RemoveAllButtons()
        {
            buttonList.Clear();
            transform.DestroyAllChilds();
            originalSelectedBtnPos = null;
            originalSelectedBtnColors = null;
        }

        /// <summary>
        /// Chiamato al click su un pulsante.
        /// </summary>
        private void OnButtonClicked(GameObject button, string text)
        {
            //Verifica se si può selezionare un pulsante, nascondendo gli altri.
            if (selectable)
            {
                if (originalSelectedBtnPos == null)
                {
                    //Non è selezionato nessun pulsante perché non è salvata nessuna posizione originale.

                    //Salva la posizione originale del pulsante cliccato, quindi lo sposta in alto.
                    originalSelectedBtnPos = button.GetComponent<RectTransform>().anchoredPosition;
                    button.GetComponent<RectTransform>().anchoredPosition = new Vector2();

                    //Salva il colore originale del pulsante cliccato, quindi lo ricolora.
                    originalSelectedBtnColors = button.GetComponent<Button>().colors;
                    button.GetComponent<Button>().colors = selectedButtonColors;

                    //Disabilita tutti gli altri pulsanti eccetto quello cliccato.
                    transform.ForEachChild(child => child.gameObject.SetActive(false));
                    button.SetActive(true);
                }
                else
                {
                    //Un pulsante è stato selezionato perché è salvata una posizione originale.

                    //Ripristina la condizione iniziale dei pulsanti.
                    button.GetComponent<RectTransform>().anchoredPosition = originalSelectedBtnPos.Value;
                    button.GetComponent<Button>().colors = originalSelectedBtnColors.Value;
                    originalSelectedBtnPos = null;
                    originalSelectedBtnColors = null;
                    transform.ForEachChild(child => child.gameObject.SetActive(true));
                }
            }

            //Invoca l'evento click.
            OnButtonClick.Invoke(text);
        }
    }
}
