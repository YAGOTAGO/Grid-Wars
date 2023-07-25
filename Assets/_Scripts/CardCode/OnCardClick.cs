using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnCardClick : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject card = eventData.pointerClick;

        CardSelectionManager.Instance.OnClickCard(card);
    }
}
