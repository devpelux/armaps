using UnityEngine;

/// <summary>
/// Gestisce gli eventi delle animazioni dei pannelli.
/// </summary>
public class PanelAnimatorEvents : StateMachineBehaviour
{
    [Tooltip("Tipo di evento.")]
    public EventType eventType;

    /// <summary>
    /// Eseguito all'ingresso nello stato.
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        OpenablePanel panel = animator.gameObject.GetComponent<OpenablePanel>();

        switch (eventType)
        {
            case EventType.BeforeOpening:
                panel.OnBeforePanelOpening();
                break;
            case EventType.AfterOpening:
                panel.OnAfterPanelOpening();
                break;
            case EventType.BeforeClosing:
                panel.OnBeforePanelClosing();
                break;
            case EventType.AfterClosing:
                panel.OnAfterPanelClosing();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Rappresenta il tipo di evento gestito.
    /// </summary>
    public enum EventType { BeforeOpening, AfterOpening, BeforeClosing, AfterClosing };
}
