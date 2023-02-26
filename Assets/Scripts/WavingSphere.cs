using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Rappresenta una sfera che si ingrandisce e riduce in un dato intervallo.
/// </summary>
public class WavingSphere : MonoBehaviour
{
    [Tooltip("Dimensione media.")]
    public float size = 1;

    [Tooltip("Ampiezza onda.")]
    public float waveAmplitude = 0;

    [Tooltip("Tempo di oscillazione.")]
    public float wavingTime = 1;

    [Tooltip("Indica quando il rigonfiamento è considerato elevato.")]
    public float highInflation = 0;

    [Tooltip("Indica il colore standard da usare.")]
    public Color normalColor = Color.red;

    [Tooltip("Indica il colore da usare quando il rigonfiamento è considerato elevato.")]
    public Color highInflationColor = Color.red;

    //Moltiplicatore del tempo, usato per "accelerare o decelerare il tempo".
    private float timeMultiplier = 0;

    private SyncronizedGameObject syncronizedGameObject = null;
    private ColoredMarker coloredMarker = null;

    //Eseguito all'avvio.
    private void Awake()
    {
        syncronizedGameObject = gameObject.GetOrAddComponent<SyncronizedGameObject>();
        coloredMarker = gameObject.GetOrAddComponent<ColoredMarker>();
    }

    //Eseguito prima degli update.
    private void Start()
    {
        //1. Converto l'oscillazione in secondi moltiplicando per 2PI (1 oscillazione = 1 secondo).
        //2. Divido per il tempo di oscillazione, infatti,
        //ad esempio: se voglio che l'oscillazione sia di 2 secondi, devo "rallentare il tempo a metà della sua velocità".
        timeMultiplier = Mathf.PI * 2 / wavingTime;

        UpdateProperties();
    }

    //Eseguito ad ogni fotogramma.
    private void Update()
    {
        UpdateProperties();
    }

    /// <summary>
    /// Calcola e imposta le proprietà in base al tempo attuale del game object.
    /// </summary>
    public void UpdateProperties()
    {
        //Rigonfiamento attuale (se negativo è uno sgonfiamento).
        float inflation = waveAmplitude * Mathf.Sin(syncronizedGameObject.GameObjectTime * timeMultiplier);

        //Calcola e imposta la dimensione attuale, che è la somma della dimensione iniziale + il rigonfiamento.
        float currentSize = size + inflation;
        transform.localScale = new Vector3(currentSize, currentSize, currentSize);

        //Imposta il colore.
        coloredMarker.color = inflation >= highInflation ? highInflationColor : normalColor; //TODO fare un "lerp" del colore??
    }
}
