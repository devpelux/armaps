using UnityEngine;
using UnityEngine.UI;

public class PathCreationManager : MonoBehaviour
{
    /// <summary>
    /// Accede all'istanza del manager.
    /// </summary>
    public static PathCreationManager Instance { get; private set; }

    public GameObject saveButton;
    public GameObject discardButton;
    public GameObject indicationsButton;

    private string source;
    private string destination;

    private PathCreationManager() { }

    /// <summary>
    /// Eseguito all'avvio.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        saveButton.GetComponent<Button>().onClick.AddListener(OnSaveClick);
        discardButton.GetComponent<Button>().onClick.AddListener(OnDiscardClick);
    }

    private void OnSaveClick()
    {
        saveButton.gameObject.SetActive(false);
        discardButton.gameObject.SetActive(false);
        indicationsButton.gameObject.SetActive(true);
        _ = MapsManager.Instance.CurrentMap.CreatePath(source, destination);
        //todo salvare i markers del percorso.
    }

    private void OnDiscardClick()
    {
        saveButton.gameObject.SetActive(false);
        discardButton.gameObject.SetActive(false);
        indicationsButton.gameObject.SetActive(true);
        //todo cancellare i markers del percorso.
    }

    public void StartCreatingPath(string source, string destination)
    {
        this.source = source;
        this.destination = destination;
        saveButton.gameObject.SetActive(true);
        discardButton.gameObject.SetActive(true);
        indicationsButton.gameObject.SetActive(false);
    }
}
