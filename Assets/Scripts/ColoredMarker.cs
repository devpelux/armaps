using UnityEngine;

/// <summary>
/// Rappresenta un marker di percorso colorato.
/// </summary>
public class ColoredMarker : MonoBehaviour
{
    [Tooltip("Colore del marker.")]
    public Color color = Color.white;

    //Renderer grafico.
    private Renderer gRenderer;

    private void Awake()
    {
        gRenderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        //Imposta il colore iniziale.
        gRenderer.material.color = color;
    }

    private void Update()
    {
        //Imposta il colore ad ogni frame.
        gRenderer.material.color = color;
    }
}
