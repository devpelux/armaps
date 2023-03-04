using UnityEngine;
using UnityEngine.UI;

namespace ARMaps.UI
{
    /// <summary>
    /// Pulsante visualizzatore del pannello delle indicazioni.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class IndicationsButton : MonoBehaviour
    {
        /// <summary>
        /// Eseguito all'avvio.
        /// </summary>
        private void Awake()
        {
            //Registra il click listener.
            GetComponent<Button>().onClick.AddListener(OnIndicationsButtonClick);
        }

        /// <summary>
        /// Eseguito al click sul pulsante.
        /// </summary>
        private void OnIndicationsButtonClick()
        {
            //Disattiva il pulsante e apre il pannello delle indicazioni.
            gameObject.SetActive(false);
            PanelInstances.IndicationsPanel.OpenPanel();
        }
    }
}
