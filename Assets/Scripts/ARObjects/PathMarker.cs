using ARMaps.Core;
using UnityEngine;

namespace ARMaps.ARObjects
{
    /// <summary>
    /// Una sfera che si ingrandisce e riduce in un dato intervallo.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class PathMarker : MonoBehaviour
    {
        [Tooltip("Dimensione media.")]
        public float size = 1;

        [Tooltip("Rigonfiamento massimo che verrà applicato.")]
        public float maxInflation = 0;

        [Tooltip("Indica quando cambiare colore per dare un segnale.")]
        public float signalInflation = 0;

        [Tooltip("Indica quando iniziare a cambiare colore per dare un segnale.")]
        public float preSignalInflation = 0;

        [Tooltip("Tempo di oscillazione (rigonfiamento <-> sgonfiamento).")]
        public float wavingTime = 1;

        [Tooltip("Ritardo dell'oscillazione (rigonfiamento <-> sgonfiamento).")]
        public float waveDelay = 0;

        [Tooltip("Colore normale.")]
        public Color normalColor = Color.white;

        [Tooltip("Colore da usare come segnale.")]
        public Color signalColor = Color.red;

        /// <summary>
        /// Eseguito prima degli update.
        /// </summary>
        private void Start()
        {
            UpdateProperties();
        }

        /// <summary>
        /// Eseguito ad ogni fotogramma.
        /// </summary>
        private void Update()
        {
            UpdateProperties();
        }

        /// <summary>
        /// Calcola e imposta le proprietà in base al tempo attuale del game object.
        /// </summary>
        private void UpdateProperties()
        {
            //Calcola una costante moltiplicativa per accelerare o decelerare le animazioni in base al tempo di oscillazione.
            //1. Converto l'oscillazione in secondi moltiplicando per 2PI (1 oscillazione = 1 secondo).
            //2. Divido per il tempo di oscillazione.
            //Ad esempio: se voglio che l'oscillazione sia di 2 secondi, devo "rallentare il tempo a metà della sua velocità".
            float timeMultiplier = Util.DOUBLE_PI / wavingTime;

            //Rigonfiamento attuale (se negativo è uno sgonfiamento).
            float inflation = maxInflation * Mathf.Sin((Time.time - waveDelay) * timeMultiplier);

            //Calcola e imposta la dimensione attuale, che è la somma della dimensione iniziale + il rigonfiamento.
            float currentSize = size + inflation;
            transform.localScale = new Vector3(currentSize, currentSize, currentSize);

            //Calcola la percentuale di tempo passata dall'inizio del punto in cui cambiare colore al termine.
            float signalStrength = (inflation - preSignalInflation) / (signalInflation - preSignalInflation);

            //Calcola il colore da impostare e lo imposta (effettua un lerp tra i colori in base a signalStrength).
            GetComponent<Renderer>().material.color = Color.Lerp(normalColor, signalColor, signalStrength);
        }
    }
}
