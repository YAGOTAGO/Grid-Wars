using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private CardBase _card; //ScriptableObject we use as data to display

    [Header("Card Elements")]
    [SerializeField] private TextMeshProUGUI _nameTMP;
    [SerializeField] private TextMeshProUGUI _descripTMP;
    [SerializeField] private Image _shape;
    [SerializeField] private VerticalLayoutGroup _keywordLayout;
    
    [Header("Keyword Window")]
    [SerializeField] private GameObject _keywordWindow;
    [SerializeField] private int _maxWidth = 100;
    [SerializeField] private int _fontSize = 10;
    private List<GameObject> _keywordWindows = new();

    private void AddKeywordWindows()
    {
        foreach(Keyword keyword in _card.Keywords)
        {
            //Instatiate a new keyword display
            GameObject window = Instantiate(_keywordWindow);
            window.SetActive(true); //do this to avoid bugs with textsize
            _keywordWindows.Add(window); //cache the window to later be able to display

            //Get components needed to size the window
            RectTransform tipWindow = window.GetComponent<RectTransform>();
            TextMeshProUGUI tmp = window.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = CardKeywords.KeywordDescriptions[keyword];
            tmp.fontSize = _fontSize;
            
            //Set size based on text
            tipWindow.sizeDelta = new Vector2(_maxWidth, tmp.preferredHeight);
            tmp.rectTransform.sizeDelta = tipWindow.sizeDelta;
            tmp.rectTransform.position = tipWindow.position;
            tipWindow.sizeDelta = new Vector2(_maxWidth, tmp.preferredHeight); //this is not repetitive

            //Set the prefab into the layoutgroup
            window.transform.SetParent(_keywordLayout.transform);
            window.SetActive(false); //necessary for window size shenanigans
         
        }

        CardKeywords.BoldenKeywords(_card);
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
        _card = card;
        AddKeywordWindows();
        UpdateDisplay(); //must be last to show bolded keyword
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
