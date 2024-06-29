using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsPreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _quantityTMP;
    [SerializeField] private TextMeshProUGUI _cardNameTMP;

    public void Initialize(int quantity, CardBase card)
    {
        _cardNameTMP.text = card.Name;
        _quantityTMP.text = $"{quantity}x";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Display the card on the mouse position

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Destroy the card

    }
}
