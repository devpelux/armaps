using UnityEngine;

namespace ARMaps.Core
{
    public static class Util
    {
        public static void DestroyAllChilds(this Transform transform)
        {
            transform.ForEachChild(child => Object.Destroy(child.gameObject));
        }

        public static void ForEachChild(this Transform transform, System.Action<Transform> action)
        {
            foreach (Transform child in transform)
            {
                action?.Invoke(child);
            }
        }
    }
}
