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

    #region Card Ability Loop
    private GameObject _selectedCardObject;
    private AbstractCard _selectedCard;
    private HexNode _clickedNode;
    private List<HexNode> _shape;

    [Header("Buttons")]
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _reselectButton;
    [SerializeField] private Button _undoButton;
    private bool _reselect;
    private bool _confirm;

    [HideInInspector] public Character ClickedCharacter;
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
        StartCoroutine(CardAbilityLoop());

    }

    private IEnumerator CardAbilityLoop()
    {
        _selectCharacterTMP.gameObject.SetActive(true); //Prompt to select character
        yield return new WaitUntil(()=> CharacterClicked()); //wait until character is selected

        
        List<AbstractAbility> abilities = _selectedCard.Abilities;
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
            
            while (true)
            {
                yield return new WaitUntil(() => ShowShape(ability, range)); //Show the shape and wait until player makes a selection

                //Ask to confirm or reselect
                _reselectButton.gameObject.SetActive(true);
                _confirmButton.gameObject.SetActive(true);

                yield return new WaitUntil(() => ButtonsClicked());

                //After clicked buttons they go away
                if (_confirm) { break; }
                ButtonsTurnOff();
            }

            ButtonsTurnOff();

            //Do the ability to the given shape
            foreach(HexNode node in _shape)
            {
                ability.DoAbility(node);
            }

            //Clear all range and target indicators
            HighlightManager.Instance.ClearTargetAndRange();
        }

        //This is after card abilities

        //Take away card durability then either destroy it or add it to the discard


    }
    private void Undo()
    {
        //Deactivate button
        _undoButton.gameObject.SetActive(false);

        _selectedCardObject.GetComponent<CardHover>().enabled = true; //Can hover again

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
        if (_reselect || _confirm)
        {
            return true;
        }
        return false;
    }

    private bool ShowShape(AbstractAbility ability, HashSet<HexNode> range)
    {
        //Clear any prior shape
        HighlightManager.Instance.ClearTargetMap();

        //If mouse node is within the range then we show the shape
        HexNode mouseNode = MouseManager.Instance.NodeMouseIsOver;
        if (range.Contains(mouseNode))
        {
            List<HexNode> shape = ability.GetShape(mouseNode);
            HighlightManager.Instance.HighlightTargetList(shape);

            if (shape.Contains(mouseNode) && NodeClicked()) //Have to check if target is valid based on type
            {
                _shape = shape;
                return true;
            }
        }

        return false;
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

    private void ButtonsTurnOff()
    {
        _confirmButton.gameObject.SetActive(false);
        _reselectButton.gameObject.SetActive(false);
        _confirm = false;
        _reselect = false;
    }
}
