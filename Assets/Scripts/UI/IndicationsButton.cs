using UnityEngine;
using UnityEngine.UI;

public class IndicationsButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(IndicationsButtonClick);
    }

    private void IndicationsButtonClick()
    {
        gameObject.SetActive(false);
        PanelInstances.IndicationsPanel.OpenPanel();
    }
}
