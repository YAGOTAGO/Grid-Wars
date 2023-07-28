using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectionManager : MonoBehaviour
{
    public static CardSelectionManager Instance;
    [Header("References")]
    [SerializeField] private Transform _selectionLocation;
    [SerializeField] private GameObject _buttons;
    [SerializeField] private TextMeshProUGUI _selectCharacterTMP;

    [Header("Tween Values")]
    [SerializeField, Range(0, 2)] private float _tweenDuration;
    [SerializeField] private Ease _ease;
    [SerializeField, Range(0,3)] private float _scale;

    #region Card Ability Loop
    private GameObject _selectedCardObject;
    private AbstractCard _selectedCard;
    private HexNode _clickedNode;
    [HideInInspector] public Character ClickedCharacter;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _buttons.SetActive(false);
        _selectCharacterTMP.gameObject.SetActive(false);
    }

    public void OnClickCard(GameObject card)
    {

        if(card == _selectedCardObject) //When click on selected card it undo selection and exit method
        {
            Undo();
            return ;
        }else if (_selectedCardObject != null) //If a different card is selected Undo before going foward
        {
            Undo();
        }

        //Make buttons visible
        _buttons.SetActive(true);

        //Cache values for checks later
        _selectedCardObject = card;

        //Card will no longer grow when hovered
        card.GetComponent<CardHover>().enabled = false;
        _selectedCard = _selectedCardObject.GetComponent<CardDisplay>().GetCard();

        //Tween to selection location and scale it up
        _selectedCardObject.transform.DOMove(_selectionLocation.position, _tweenDuration).SetEase(_ease);
        _selectedCardObject.transform.DOScale(new Vector3(_scale, _scale), _tweenDuration);

        //Call the beginning ability of card
        //for each ability in card ability list
        //Start that coroutine

        //ie: walk damage 3 push 2

        //

    }

    private IEnumerator CardAbilityLoop()
    {
        _selectCharacterTMP.gameObject.SetActive(true); //Prompt to select character
        yield return new WaitUntil(()=> CharacterClicked()); //wait until character is selected

        //Display range if possible
        if(_selectedCard.Range > 0)
        {
            HashSet<HexNode> range = BFS.BFSWalkable(ClickedCharacter.NodeOn, _selectedCard.Range);
        }

    }

    private bool CharacterClicked()
    {
        if (NodeClicked() && _clickedNode.CharacterOnNode != null)
        {
            ClickedCharacter = _clickedNode.CharacterOnNode;
            _selectCharacterTMP.gameObject.SetActive(false);
            return true;
        }

        return false;
    }

    //returns whether a node was clicked and sets the _clickedNode
    private bool NodeClicked()
    {
        //if mouse is not over UI and the button is clicked then we have clicked a node
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            _clickedNode = MouseManager.Instance.NodeMouseIsOver;
            return true;
        }

        return false;
    }

    public void Undo()
    {
        //Deactivate button
        _buttons.SetActive(false);
        
        _selectedCardObject.GetComponent<CardHover>().enabled = true; //Can hover again

        //Find what slot the game object belongs to
        Transform cardSlot = DeckManager.Instance.GetSlotTransformFromCard(_selectedCardObject);
        
        //Tween back to slot and scale card down to 1
        _selectedCardObject.transform.DOMove(cardSlot.position, _tweenDuration).SetEase(_ease);
        _selectedCardObject.transform.DOScale(new Vector3(1, 1), _tweenDuration);

        _selectedCardObject = null; //Unselect card
        _selectedCard = null;
    }
}
