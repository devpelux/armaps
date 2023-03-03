using ARMaps.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SearchPanel : MonoBehaviour
{
    public GameObject searchBoxInput;
    public GameObject resultContainer;

    public GameObject resultButtonPrefab;

    public bool selectableResult;

    public string Text => searchBoxInput.GetComponent<TMP_InputField>().text;

    public UnityEvent<string> OnSearchTextChange { get; } = new();
    public UnityEvent<string> OnResultClick { get; } = new();

    private int resultButtonCount = 0;

    private bool resultSelected = false;
    private Vector2 btnPosCache;

    private void Awake()
    {
        searchBoxInput.GetComponent<TMP_InputField>().onValueChanged.AddListener(OnSearchBoxInputValueChanged);
    }

    public void AddResultButton(string text)
    {
        GameObject resultButton = Instantiate(resultButtonPrefab, resultContainer.transform);
        resultButton.transform.GetChild(0).GetComponent<TMP_Text>().text = text;
        resultButton.GetComponent<Button>().onClick.AddListener(() => OnResultButtonClick(text, resultButton));
        resultButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20 * resultButtonCount);
        resultButtonCount++;
    }

    public void ClearResultButtons()
    {
        resultContainer.transform.DestroyAllChilds();
        resultButtonCount = 0;
        resultSelected = false;
        searchBoxInput.GetComponent<TMP_InputField>().readOnly = false;
    }

    public void Clear()
    {
        searchBoxInput.GetComponent<TMP_InputField>().text = "";
        ClearResultButtons();
        OnSearchBoxInputValueChanged("");
    }

    private void OnSearchBoxInputValueChanged(string text)
    {
        OnSearchTextChange.Invoke(text);
    }

    private void OnResultButtonClick(string text, GameObject button)
    {
        if (selectableResult)
        {
            resultSelected = !resultSelected;
            if (resultSelected)
            {
                btnPosCache = button.GetComponent<RectTransform>().anchoredPosition;
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2();
                resultContainer.transform.ForEachChild(child => child.gameObject.SetActive(false));
                button.SetActive(true);
                searchBoxInput.GetComponent<TMP_InputField>().readOnly = true;
            }
            else
            {
                button.GetComponent<RectTransform>().anchoredPosition = btnPosCache;
                resultContainer.transform.ForEachChild(child => child.gameObject.SetActive(true));
                searchBoxInput.GetComponent<TMP_InputField>().readOnly = false;
            }
        }
        OnResultClick.Invoke(text);
    }
}
