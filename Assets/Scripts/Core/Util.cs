using UnityEngine;
using UnityEngine.Events;

namespace ARMaps.Core
{
    /// <summary>
    /// Utility.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Due volte pi greco.
        /// </summary>
        public const float DOUBLE_PI = Mathf.PI * 2;

        /// <summary>
        /// Distrugge tutti i figli della transform.
        /// </summary>
        public static void DestroyAllChilds(this Transform transform)
        {
            transform.ForEachChild(child => Object.Destroy(child.gameObject));
        }

        /// <summary>
        /// Esegue l'azione specificata su tutti i figli della transform.
        /// </summary>
        public static void ForEachChild(this Transform transform, UnityAction<Transform> action)
        {
            foreach (Transform child in transform)
            {
                action?.Invoke(child);
            }
        }

        /// <summary>
        /// Compara due stringhe con il modo <see cref="System.StringComparison.OrdinalIgnoreCase"/>.
        /// </summary>
        public static bool EqualsInsensitive(this string str, string str2)
        {
            return str.Equals(str2, System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
