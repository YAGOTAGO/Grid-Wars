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
    [SerializeField] private TextMeshProUGUI _promptTMP;

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
    [HideInInspector] public AbstractCharacter ClickedCharacter;
    private Coroutine _cardLoopCoroutine; //store this to cancel it later
    private bool _canStopCoroutine = true;
    private HexNode _priorMouseNode;
    public List<HexNode> BreakPoints = new();
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _undoButton.gameObject.SetActive(false);
        Prompt("", false);
        ButtonsSetActive(false);
        _confirmButton.onClick.AddListener(() => _confirm = true);
        _reselectButton.onClick.AddListener(() => _reselect = true);
    }

    public void OnClickCard(GameObject card)
    {
        if (!_canStopCoroutine) { return; } //Stops selecting a new card after an action has taken place

        if (card == _selectedCardObject) //When click on selected card it undo selection and exit method
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
        TweenManager.Instance.CardMove(_selectedCardObject, _selectionLocation.position);
        TweenManager.Instance.CardScale(_selectedCardObject, TweenManager.Instance.CardScaleUp);

        //Start that coroutine for card abilities
        _cardLoopCoroutine = StartCoroutine(CardAbilityLoop(_selectedCard));

    }

    private IEnumerator CardAbilityLoop(AbstractCard card)
    {
        Prompt("Select a Character", true);
        yield return new WaitUntil(() => CharacterClicked()); //wait until character is selected

        List<AbstractAbility> abilities = card.Abilities;
        HashSet<HexNode> range = new();

        for (int i = 0; i < abilities.Count; i++) //iterate over all abilities in the card
        {
            //Before starting any ability we wait for action queue
            yield return new WaitUntil(ActionQueue.Instance.IsQueueStopped);

            Prompt(abilities[i].Prompt, true); //Tell the player what to expect

            TargetingType tarType = abilities[i].GetTargetingType();

            switch (tarType)
            {
                case TargetingType.AIREAL:
                    range = BFS.BFSAll(ClickedCharacter.NodeOn, abilities[i].Range);
                    break;
                case TargetingType.NORMAL:
                    range = BFS.BFSNormalAbilties(ClickedCharacter.NodeOn, abilities[i].Range);
                    break;
                case TargetingType.WALKABLE:
                    range = BFS.BFSWalkable(ClickedCharacter.NodeOn, abilities[i].Range);
                    break;
                case TargetingType.NONE:
                case TargetingType.SELF:
                    if (i > 0) //If we already in the loop then we dont need to use confirm button
                    {
                        abilities[i].DoAbility(ClickedCharacter.NodeOn);
                        CannotStopCoroutine();
                        continue;
                    }
                    _confirmButton.gameObject.SetActive(true);
                    yield return new WaitUntil(() => ButtonsClicked()); //wait for confirm
                    abilities[i].DoAbility(ClickedCharacter.NodeOn); //Pass the node character is on
                    CannotStopCoroutine();
                    _confirm = false;
                    _confirmButton.gameObject.SetActive(false);
                    continue;
            }

            HighlightManager.Instance.HighlightRangeSet(range);

            yield return null; //have to wait a frame before trying to detect click again

            //loops if click reselect button
            while (true)
            {
                yield return new WaitUntil(() => ShowShape(abilities[i], range)); //Show the shape and wait until player makes a selection

                //Ask to confirm or reselect
                ButtonsSetActive(true);
                yield return new WaitUntil(() => ButtonsClicked());

                //After clicked buttons they go away
                if (_confirm)
                {
                    ButtonsSetActive(false);
                    Prompt("", false);
                    CannotStopCoroutine();
                    break;
                }
                Prompt("", false);
                ButtonsSetActive(false); //this is not redudant KEEP IT
            }

            //Do the ability to the given shape 
            foreach (HexNode node in _shape)
            {
                abilities[i].DoAbility(node);
            }

            //Clear all range and target indicators
            HighlightManager.Instance.ClearTargetAndRange();
        }

        //Take away card durability then either destroy it or add it to the discard
        if (card.Rarity != Rarity.BASIC) { card.Durability--;}
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
        //This is after cards abilities
        _canStopCoroutine = true;

    }
    private void Undo()
    {
        //If we can cancel coroutine
        if(!_canStopCoroutine){ return; }

        StopCoroutine(_cardLoopCoroutine);

        //Get rid of reselect and confirm buttons
        ButtonsSetActive(false);

        //Remove the range tile map
        HighlightManager.Instance.ClearTargetAndRange();

        //Deactivate the Select character text
        Prompt("", false);

        //Deactivate undo button
        _undoButton.gameObject.SetActive(false);

        //Can hover again
        _selectedCardObject.GetComponent<CardHover>().enabled = true; 

        //Find what slot the game object belongs to
        Transform cardSlot = DeckManager.Instance.GetSlotTransformFromCard(_selectedCardObject);
        
        //Tween back to slot and scale card down to 1
        TweenManager.Instance.CardMove(_selectedCardObject, cardSlot.position);
        TweenManager.Instance.CardScale(_selectedCardObject, 1f);

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
        HexNode mouseNode = MouseManager.Instance.NodeMouseIsOver;

        //If mouse node is within the range then we show the shape
        if (range.Contains(mouseNode))
        {

            if(mouseNode != _priorMouseNode) //only find shape if mouse node changes
            {
                HighlightManager.Instance.ClearTargetMap(); //Clear any prior shape
                _shape = ability.GetShape(mouseNode);
                HighlightManager.Instance.HighlightTargetList(_shape);
            }

            if (_shape.Contains(mouseNode) && NodeClicked()) //Have to check if target is valid based on type
            {
                _priorMouseNode = mouseNode;
                BreakPoints.Clear();
                return true;
            }
        }

        _priorMouseNode = mouseNode;
        return false;
    }

    private bool CharacterClicked()
    {

        if (NodeClicked() && _clickedNode.CharacterOnNode != null)
        {
            ClickedCharacter = _clickedNode.CharacterOnNode;
            _promptTMP.gameObject.SetActive(false);
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

    private void ButtonsSetActive(bool isActive)
    {
        _confirmButton.gameObject.SetActive(isActive);
        _reselectButton.gameObject.SetActive(isActive);
        _confirm = false;
        _reselect = false;
    }

    private void Prompt(string text, bool setActive)
    {
        _promptTMP.text = text;
        _promptTMP.gameObject.SetActive(setActive);
    }

    private void CannotStopCoroutine()
    {
        _canStopCoroutine = false;
        _undoButton.gameObject.SetActive(false);
    }
}
