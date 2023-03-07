using UnityEngine;

namespace ARMaps.UI
{
    /// <summary>
    /// Raccoglie le istanze dei pannelli per potervi accedere facilmente globalmente.
    /// </summary>
    public class PanelInstances : MonoBehaviour
    {
        [Tooltip("Pannello delle indicazioni.")]
        [SerializeField] private IndicationsPanel indicationsPanel;

        [Tooltip("Pannello delle mappe.")]
        [SerializeField] private MapsPanel mapsPanel;

        /// <summary>
        /// Ottiene l'istanza del pannello delle indicazioni.
        /// </summary>
        public static IndicationsPanel IndicationsPanel => instance.indicationsPanel;

        /// <summary>
        /// Ottiene l'istanza del pannello delle mappe.
        /// </summary>
        public static MapsPanel MapsPanel => instance.mapsPanel;

        /// <summary>
        /// Istanza del raccoglitore.
        /// </summary>
        private static PanelInstances instance;

        /// <summary>
        /// Inizializza PanelInstances.
        /// </summary>
        private PanelInstances() { }

        /// <summary>
        /// Eseguito all'avvio.
        /// </summary>
        private void Awake() => instance = instance == null ? this : instance;
    }
}
