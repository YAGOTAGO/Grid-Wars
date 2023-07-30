using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    private AbstractCard _card; //ScriptableObject we use as data to display

    [SerializeField] private TextMeshProUGUI _nameTMP;
    [SerializeField] private TextMeshProUGUI _rangeTMP;
    [SerializeField] private TextMeshProUGUI _descripTMP;
    [SerializeField] private Image _shape;
    
    private void SetCard(AbstractCard card)
    {
        _card = card;
    }

    private void UpdateDisplay()
    {
        _nameTMP.text = _card.Name.ToString();
        _rangeTMP.text = _card.Range.ToString();
        _descripTMP.text = _card.Description;
        _shape.sprite = _card.ShapeArt;
    }

    //Call this to initialize new disaplay
    public void Initialize(AbstractCard card)
    {
        SetCard(card);
        UpdateDisplay();
    }

    public AbstractCard GetCard() { return _card; }

 //   public AbstractAbility ReturnAbility() { return _card.Ability; }
}
