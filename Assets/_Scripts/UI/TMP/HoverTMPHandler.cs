using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverTMPHandler : MonoBehaviour
{

    private GameObject _card;
    [SerializeField] private Canvas _canvas;

    private void OnEnable()
    {
        LinkHandlerTMP.OnHoverOnLinkEvent += DisplayCard;
        LinkHandlerTMP.OnCloseTooltipEvent += CloseCardDisplay;
    }

    private void OnDisable()
    {
        LinkHandlerTMP.OnHoverOnLinkEvent -= DisplayCard;
        LinkHandlerTMP.OnCloseTooltipEvent -= CloseCardDisplay;
    }

    private void DisplayCard(string cardName, Vector3 mousePos)
    {
        if(_card != null) { return ; }
        Vector3 spawnPos = mousePos + new Vector3(100, -50, 0);
        _card = DeckManager.Instance.InstantiateCard(Database.Instance.GetCardByName(cardName), spawnPos);
        _card.GetComponent<OnCardClick>().enabled = false;
        _card.GetComponent<CardHover>().enabled = false;
        _card.transform.SetParent(_canvas.transform);
    }

    public void CloseCardDisplay()
    {
        Destroy(_card);
    }
}
