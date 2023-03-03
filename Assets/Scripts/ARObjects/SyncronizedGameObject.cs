using UnityEngine;

/// <summary>
/// Rappresenta un game object sincronizzato, che calcola il suo tempo in base a un certo scostamento dal tempo globale.
/// </summary>
public class SyncronizedGameObject : MonoBehaviour
{
    /// <summary>
    /// Indica il tempo locale di questo game object, sottraendo il tempo globale dallo scostamento.
    /// Quindi uno scostamento positivo, ad esempio, sposterà indietro il tempo, ritardando gli eventi.
    /// </summary>
    public float GameObjectTime => Time.time - TimeShift;

    /// <summary>
    /// Indica di quanto scostare il tempo dal tempo globale.
    /// Valori positivi ritardano le animazioni, i negativi le anticipano.
    /// </summary>
    public float TimeShift { get; set; } = 0;
}
