using TMPro;
using UnityEngine;

public class MapName : MonoBehaviour
{
    public string startText;

    public void SetMapName(string name)
    {
        GetComponent<TMP_Text>().text = startText + " " + name;
    }
}
