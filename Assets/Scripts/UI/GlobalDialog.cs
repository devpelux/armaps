using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Gestisce la dialog "YES / NO" globale.
/// </summary>
public class GlobalDialog : MonoBehaviour
{
    [Tooltip("Pulsante YES.")]
    [SerializeField] private Button yesButton;

    [Tooltip("Pulsante NO.")]
    [SerializeField] private Button noButton;

    [Tooltip("Casella di testo del titolo.")]
    [SerializeField] private TMP_Text titleText;

    [Tooltip("Casella di testo del messaggio.")]
    [SerializeField] private TMP_Text messageText;

    /// <summary>
    /// Il chiamante della dialog dovrà inviare questa azione che verrà eseguita al termine.
    /// </summary>
    private UnityAction<Result> onExit = null;

    /// <summary>
    /// Accede all'istanza della dialog "YES / NO" globale.
    /// </summary>
    public static GlobalDialog Instance { get; private set; } = null;

    /// <summary>
    /// Inizializza la global dialog.
    /// </summary>
    private GlobalDialog() { }

    /// <summary>
    /// Eseguito all'avvio.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            yesButton.onClick.AddListener(OnYesClick);
            noButton.onClick.AddListener(OnNoClick);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Eseguito al click sul pulsante YES.
    /// </summary>
    private void OnYesClick() => SendResultAndClose(Result.YES);

    /// <summary>
    /// Eseguito al click sul pulsante NO.
    /// </summary>
    private void OnNoClick() => SendResultAndClose(Result.NO);

    /// <summary>
    /// Chiude la dialog e invia il risultato al chiamante.
    /// </summary>
    private void SendResultAndClose(Result result)
    {
        //Chiude la dialog.
        gameObject.SetActive(false);

        //Invia il risultato, quindi elimina l'azione  perché non serve più.
        onExit?.Invoke(result);
        onExit = null;
    }

    /// <summary>
    /// Attiva la dialog impostando il titolo, il messaggio, e un'azione che verrà eseguita all'uscita.
    /// </summary>
    public void Call(string title, string message, UnityAction<Result> onExit)
    {
        //Imposta il titolo, il messaggio, e l'azione da eseguire all'uscita.
        titleText.text = title;
        messageText.text = message;
        this.onExit = onExit;

        //Attiva la dialog.
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Rappresenta i possibili risultati della dialog globale.
    /// </summary>
    public enum Result { YES, NO }
}
