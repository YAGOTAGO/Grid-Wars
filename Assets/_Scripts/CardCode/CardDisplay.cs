using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private CardBase _card; //ScriptableObject we use as data to display

    [SerializeField] private TextMeshProUGUI _nameTMP;
    [SerializeField] private TextMeshProUGUI _descripTMP;
    [SerializeField] private Image _shape;
    [SerializeField] private VerticalLayoutGroup _keywordLayout;
    
    [Header("Keyword Window")]
    [SerializeField] private GameObject _keywordWindow;
    [SerializeField] private int _maxWidth = 100;
    [SerializeField] private int _fontSize = 10;
    private List<GameObject> _keywordWindows = new();
    private void SetCard(CardBase card)
    {
        _card = card;
    }

    private void AddKeywordWindows()
    {
        foreach(Keyword keyword in _card.Keywords)
        {
            //Instatiate a new keyword display
            GameObject window = Instantiate(_keywordWindow);
            window.SetActive(true);
            _keywordWindows.Add(window);

            RectTransform tipWindow = window.GetComponent<RectTransform>();
            TextMeshProUGUI tmp = window.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = CardKeywords.KeywordDescriptions[keyword];
            tmp.fontSize = _fontSize;
            
            //Set size based on text
            tipWindow.sizeDelta = new Vector2(_maxWidth, tmp.preferredHeight);
            tmp.rectTransform.sizeDelta = tipWindow.sizeDelta;
            tmp.rectTransform.position = tipWindow.position;
            tipWindow.sizeDelta = new Vector2(_maxWidth, tmp.preferredHeight); //this is not repetitive

            //Set the position
            window.transform.SetParent(_keywordLayout.transform);
            window.SetActive(false); //necessary for window size shenanigans
        }
    }

    private void UpdateDisplay()
    {
        _nameTMP.text = _card.Name.ToString();
        _descripTMP.text = _card.Description;
        _shape.sprite = _card.ShapeArt;
    }

    //Call this to initialize new disaplay
    public void Initialize(CardBase card)
    {
        SetCard(card);
        UpdateDisplay();
        AddKeywordWindows();
    }

    public CardBase GetCard() { return _card; }

    public void DisplayKeyword(bool display)
    {
        foreach(GameObject go in _keywordWindows)
        {
            go.SetActive(display);
        }
    }

}
