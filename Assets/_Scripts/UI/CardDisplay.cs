using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private Card _card; //ScriptableObject we use as data to display

    [SerializeField] private TextMeshProUGUI _nameTMP;
    [SerializeField] private TextMeshProUGUI _rangeTMP;
    [SerializeField] private TextMeshProUGUI _descripTMP;
    [SerializeField] private Image _shape;
    
    private void SetCard(Card card)
    {
        _card = card;
    }

    private void UpdateDisplay()
    {
        _nameTMP.text = _card.Name;
        _rangeTMP.text = _card.Range.ToString();
        _descripTMP.text = _card.Description;
        _shape.sprite = _card.Shape;
    }

    //Call this to initialize new disaplay
    public void Initialize(Card card)
    {
        SetCard(card);
        UpdateDisplay();
    }
}
