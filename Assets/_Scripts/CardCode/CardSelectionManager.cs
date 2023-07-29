using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSelectionManager : MonoBehaviour
{
    public static CardSelectionManager Instance;
    [Header("References")]
    [SerializeField] private Transform _selectionLocation;
    [SerializeField] private TextMeshProUGUI _selectCharacterTMP;

    [Header("Tween Values")]
    [SerializeField, Range(0, 2)] private float _tweenDuration;
    [SerializeField] private Ease _ease;
    [SerializeField, Range(0,3)] private float _scale;

    [Header("Buttons")]
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _reselectButton;
    [SerializeField] private Button _undoButton;
    private bool _reselect;
    private bool _confirm;

    #region Card Ability Loop
    private GameObject _selectedCardObject; //game object so we can potentially destroy card
    private AbstractCard _selectedCard; //The information of what the card does
    private HexNode _clickedNode; //Node we clicked on
    private List<HexNode> _shape; //Shape we last hovered
    [HideInInspector] public Character ClickedCharacter;
    private Coroutine _cardLoopCoroutine; //store this to cancel it later
    private bool _canStopCoroutine = true;
    private HexNode _priorMouseNode;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _undoButton.gameObject.SetActive(false);
        _selectCharacterTMP.gameObject.SetActive(false);
        ButtonsTurnOff();
        _confirmButton.onClick.AddListener(() => _confirm = true);
        _reselectButton.onClick.AddListener(() => _reselect = true);
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
        _undoButton.gameObject.SetActive(true);

        //Cache values for checks later
        _selectedCardObject = card;

        //Card will no longer grow when hovered
        card.GetComponent<CardHover>().enabled = false;
        _selectedCard = _selectedCardObject.GetComponent<CardDisplay>().GetCard();

        //Tween to selection location and scale it up
        _selectedCardObject.transform.DOMove(_selectionLocation.position, _tweenDuration).SetEase(_ease);
        _selectedCardObject.transform.DOScale(new Vector3(_scale, _scale), _tweenDuration);

        //Start that coroutine for card abilities
        _cardLoopCoroutine = StartCoroutine(CardAbilityLoop(_selectedCard));

    }

    private IEnumerator CardAbilityLoop(AbstractCard card)
    {
        Debug.Log("Card ability loop started");

        _selectCharacterTMP.gameObject.SetActive(true); //Prompt to select character
        yield return new WaitUntil(()=> CharacterClicked()); //wait until character is selected

        List<AbstractAbility> abilities = card.Abilities;
        HashSet<HexNode> range = new();
        
        //Display range if possible
        foreach (AbstractAbility ability in abilities)
        {
            if (ability.Range > 0)
            {

                TargetingType tarType = ability.GetTargetingType();

                switch (tarType)
                {
                    case TargetingType.AIREAL:
                        range = BFS.BFSAll(ClickedCharacter.NodeOn, ability.Range);
                        break;
                    case TargetingType.NORMAL:
                        range = BFS.BFSNormalAbilties(ClickedCharacter.NodeOn, ability.Range);
                        break;
                    case TargetingType.WALKABLE:
                        range = BFS.BFSWalkable(ClickedCharacter.NodeOn, ability.Range);
                        break;
                }

            }

            HighlightManager.Instance.HighlightRangeSet(range);
            yield return null; //have to wait a frame before trying to detect click again
            
            //loops so reselect works
            while (true)
            {
                yield return new WaitUntil(() => ShowShape(ability, range)); //Show the shape and wait until player makes a selection

                //Ask to confirm or reselect
                _reselectButton.gameObject.SetActive(true);
                _confirmButton.gameObject.SetActive(true);

                yield return new WaitUntil(() => ButtonsClicked());

                //After clicked buttons they go away
                if (_confirm) 
                {
                    ButtonsTurnOff();
                    _undoButton.gameObject.SetActive(false);
                    _canStopCoroutine = false;
                    break; 
                }
                ButtonsTurnOff(); //this is not redudant KEEP IT
            }

            //Do the ability to the given shape
            foreach(HexNode node in _shape)
            {
                ability.DoAbility(node);
            }

            //Clear all range and target indicators
            HighlightManager.Instance.ClearTargetAndRange();
        }

        //This is after cards abilities
        _canStopCoroutine = true;

        //Take away card durability then either destroy it or add it to the discard
        card.Durability--;
        if(card.Durability <= 0)
        {
            //remove card from the hand
            DeckManager.Instance.RemoveFromHand(_selectedCardObject, true);
        }
        else
        {
            //Move from hand to discard
            DeckManager.Instance.HandCardToDiscard(_selectedCardObject);
        }

    }
    private void Undo()
    {
        //If we can cancel coroutine
        if(_canStopCoroutine){ StopCoroutine(_cardLoopCoroutine); }

        //Get rid of reselect and confirm buttons
        ButtonsTurnOff();

        //Remove the range tile map
        HighlightManager.Instance.ClearTargetAndRange();

        //Deactivate the Select character text
        _selectCharacterTMP.gameObject.SetActive(false);

        //Deactivate undo button
        _undoButton.gameObject.SetActive(false);

        //Can hover again
        _selectedCardObject.GetComponent<CardHover>().enabled = true; 

        //Find what slot the game object belongs to
        Transform cardSlot = DeckManager.Instance.GetSlotTransformFromCard(_selectedCardObject);
        //Tween back to slot and scale card down to 1
        _selectedCardObject.transform.DOMove(cardSlot.position, _tweenDuration).SetEase(_ease);
        _selectedCardObject.transform.DOScale(new Vector3(1, 1), _tweenDuration);
        _selectedCardObject = null; //Unselect card
        _selectedCard = null;
    }

    private bool ButtonsClicked()
    {
        Debug.Log("calling buttons clicked");

        if (_reselect || _confirm)
        {
            return true;
        }
        return false;
    }

    private bool ShowShape(AbstractAbility ability, HashSet<HexNode> range)
    {
        HexNode mouseNode = MouseManager.Instance.NodeMouseIsOver;

        //If mouse node is within the range then we show the shape
        if (range.Contains(mouseNode))
        {

            if(mouseNode != _priorMouseNode) //only find shape if mouse node changes
            {
                HighlightManager.Instance.ClearTargetMap(); //Clear any prior shape
                _shape = ability.GetShape(mouseNode);
                HighlightManager.Instance.HighlightTargetList(_shape);
                Debug.Log("calling show shape");
            }

            Debug.Log(_shape.Contains(mouseNode));

            if (_shape.Contains(mouseNode) && NodeClicked()) //Have to check if target is valid based on type
            {
                _priorMouseNode = mouseNode;
                return true;
            }
        }

        _priorMouseNode = mouseNode;
        return false;
    }

    private bool CharacterClicked()
    {
        Debug.Log("calling character clicked");

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
        Debug.Log("calling Node Clicked");
        //if mouse is not over UI and the button is clicked then we have clicked a node
        if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
        {
            _clickedNode = MouseManager.Instance.NodeMouseIsOver;
            return true;
        }

        return false;
    }

    private void ButtonsTurnOff()
    {
        _confirmButton.gameObject.SetActive(false);
        _reselectButton.gameObject.SetActive(false);
        _confirm = false;
        _reselect = false;
    }
}
