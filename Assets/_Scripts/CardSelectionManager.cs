using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectionManager : MonoBehaviour
{
    public static CardSelectionManager Instance;
    [SerializeField] private Transform _selectionLocation;
    [SerializeField] private GameObject _buttons;

    private GameObject _selectedCard;

    [Header("Tween Values")]
    [SerializeField, Range(0, 2)] private float _tweenDuration;
    [SerializeField] private Ease _ease;
    [SerializeField, Range(0,3)] private float _scale;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _buttons.SetActive(false);
    }

    public void OnClickCard(GameObject card)
    {

        if(card == _selectedCard) //When click on selected card it undo selection and exit method
        {
            Undo();
            return ;
        }else if (_selectedCard != null) //If a different card is selected Undo before going foward
        {
            Undo();
        }

        //Make buttons visible
        _buttons.SetActive(true);

        //Cache values for checks later
        _selectedCard = card;

        //Card will no longer grow when hovered
        card.GetComponent<CardHover>().enabled = false;

        //Tween to selection location and scale it up
        _selectedCard.transform.DOMove(_selectionLocation.position, _tweenDuration).SetEase(_ease);
        _selectedCard.transform.DOScale(new Vector3(_scale, _scale), _tweenDuration);

        //Call the begginning ability of card

    }

    public void Undo()
    {
        //Deactivate button
        _buttons.SetActive(false);
        
        _selectedCard.GetComponent<CardHover>().enabled = true; //Can hover again

        //Find what slot the game object belongs to
        Transform cardSlot = DeckManager.Instance.GetSlotTransformFromCard(_selectedCard);
        
        //Tween back to slot and scale card down to 1
        _selectedCard.transform.DOMove(cardSlot.position, _tweenDuration).SetEase(_ease);
        _selectedCard.transform.DOScale(new Vector3(1, 1), _tweenDuration);

        _selectedCard = null; //Unselect card
    }
}
