using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnCardClick : MonoBehaviour, IPointerClickHandler
{

    private bool _isForReward;
    private bool _isRewardCardClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject card = eventData.pointerClick;

        if (_isForReward) //select as a reward
        {
            //Do the logic of a card reward being clicked
            _isRewardCardClicked = true;
        }
        else
        {
            CardSelectionManager.Instance.OnClickCard(card); //if in hand
        }
        
    }

    public bool IsCardRewardClicked()
    {
        return _isRewardCardClicked;
    }

    public void CardIsForReward()
    {
        _isForReward = true;
    }
}
