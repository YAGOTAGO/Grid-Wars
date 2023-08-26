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
    
    private void SetCard(CardBase card)
    {
        _card = card;
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
    }

    public CardBase GetCard() { return _card; }

 //   public AbstractAbility ReturnAbility() { return _card.Ability; }
}
