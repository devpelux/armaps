using UnityEngine;

/// <summary>
/// Raccoglie le istanze dei pannelli per potervi accedere facilmente globalmente.
/// </summary>
public class PanelInstances : MonoBehaviour
{
    [Tooltip("Pannello delle indicazioni.")]
    public IndicationsPanel indicationsPanel;

    [Tooltip("Pannello delle mappe.")]
    public MapsPanel mapsPanel;

    /// <summary>
    /// Pannello delle indicazioni.
    /// </summary>
    public static IndicationsPanel IndicationsPanel => instance.indicationsPanel;

    /// <summary>
    /// Pannello delle mappe.
    /// </summary>
    public static MapsPanel MapsPanel => instance.mapsPanel;

    private static PanelInstances instance;

    private void Awake() => instance = this;
}
