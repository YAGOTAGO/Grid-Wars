using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSelectionManager : MonoBehaviour
{
    public static CardSelectionManager Instance;
    [Header("References")]
    [SerializeField] private Transform _selectionLocation;
    [SerializeField] private TextMeshPro _promptTMP;

    [Header("Buttons")]
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _reselectButton;
    [SerializeField] private Button _skipButton;
    [SerializeField] private Button _undoButton;
    private bool _reselect;
    private bool _confirm;
    private bool _skip;

    #region Card Ability Loop
    private GameObject _selectedCardObject; //game object so we can potentially destroy card
    public CardBase SelectedCard; //The information of what the card does
    private HexNode _clickedNode; //Node we clicked on
    private List<HexNode> _shape = new(); //Shape we last hovered
    public AbstractCharacter SelectedCharacter;
    private Coroutine _cardLoopCoroutine; //store this to cancel it later
    [SerializeField]private bool _canStopCoroutine = true;
    private HexNode _priorMouseNode;
    #endregion

    #region Walkable Shape
    private List<HexNode> _walkShape = new();
    private HexNode _walkHex;
    private List<HexNode> _walkTemp = new(); //used to show the highlight
    #endregion
    void Start()
    {
        Instance = this;
        _undoButton.gameObject.SetActive(false);
        Prompt("", false);
        ButtonsSetActive(false, false, false); //no buttons are active

        //Add listeners so buttons do things
        _confirmButton.onClick.AddListener(() => _confirm = true);
        _reselectButton.onClick.AddListener(() => _reselect = true);
        _skipButton.onClick.AddListener(() => _skip = true);

    }

    public void OnClickCard(GameObject card)
    {
        if (!_canStopCoroutine) { return; } //Stops selecting a new card after an action has taken place

        if (!EndTurnButton.Instance.IsItMyTurn())
        {
            PopUp.Instance.PopUpText("It is NOT your turn.");
            return;
        }

        EndTurnButton.Instance.CanClickEndTurn(false);
        
        card.GetComponent<CardDisplay>().DisplayKeyword(false); //Selected card removes keyword display

        if (card == _selectedCardObject) //When click on same card undo selection
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
        SelectedCard = _selectedCardObject.GetComponent<CardDisplay>().GetCard();

        //Tween to selection location and scale it up
        TweenManager.Instance.CardMove(_selectedCardObject, _selectionLocation.position);
        TweenManager.Instance.CardScale(_selectedCardObject, true);

        //Start that coroutine for card abilities
        _cardLoopCoroutine = StartCoroutine(CardAbilityLoop(SelectedCard));

    }

    private IEnumerator CardAbilityLoop(CardBase card)
    {
        Prompt("Select a Character", true);
        yield return new WaitUntil(() => CharacterClicked()); //wait until character is selected

        List<AbilityBase> abilities = card.Abilities;
        List<HexNode> range = new();

        for (int i = 0; i < abilities.Count; i++) //iterate over all abilities in the card
        {

            yield return new WaitUntil(ActionQueue.Instance.IsQueueStopped); //Before starting any ability we wait for action queue

            Prompt(abilities[i].Prompt, true); //Tell the player what to expect

            TargetingType tarType = abilities[i].GetTargetingType();

            switch (tarType)
            {
                case TargetingType.AIREAL:
                case TargetingType.NORMAL:
                case TargetingType.WALKABLE:
                    range = abilities[i].Shape.Range(SelectedCharacter.GetNodeOn(), abilities[i]);
                    break;
                case TargetingType.NONE:
                case TargetingType.SELF:
                    if (i > 0) //If we already in the loop then we dont need to use confirm button
                    {
                        ButtonsSetActive(true, false, true); //we can confirm or skip
                        yield return new WaitUntil(() => ButtonsClicked());
                        if (_skip) //if skip we dont do ability
                        {
                            ButtonsSetActive(false, false, false);
                            Prompt("", false);
                            continue;
                        }

                        StartCoroutine(abilities[i].DoAbility(new List<HexNode> { SelectedCharacter.GetNodeOn() }, card)); //do ability if not skip
                        ButtonsSetActive(false, false, false); //get rid of the UI stuff
                        Prompt("", false);
                        continue;
                    }

                    ButtonsSetActive(true, false, true);
                    yield return new WaitUntil(() => ButtonsClicked()); //wait for confirm
                    if (_skip) //if skip we dont do ability
                    {
                        ButtonsSetActive(false, false, false);
                        CanStopCoroutine(false);
                        Prompt("", false);
                        continue;
                    }
                    StartCoroutine(abilities[i].DoAbility(new List<HexNode> { SelectedCharacter.GetNodeOn() }, card)); //Pass the node character is on
                    CanStopCoroutine(false);
                    ButtonsSetActive(false, false, false);
                    Prompt("", false);
                    continue;
            }

            HighlightManager.Instance.HighlightRangeList(range);

            yield return null; //have to wait a frame before trying to detect click again

            //loops if click reselect button
            while (true)
            {
                if (abilities[i].IsSpecialPathfind) { AdditivePrompt("May right click to add breakpoints.");  }
                yield return new WaitUntil(() => ShowShape(abilities[i], range)); //Show the shape and wait until player makes a selection

                //Ask to confirm or reselect
                ButtonsSetActive(true, true, true);
                yield return new WaitUntil(() => ButtonsClicked());

                //After clicked buttons they go away
                if (_confirm)
                {
                    ButtonsSetActive(false, false, false);
                    Prompt("", false);
                    CanStopCoroutine(false);

                    //Clear all range and target indicators
                    HighlightManager.Instance.ClearTargetAndRange();

                    //Do ability when we confirm and wait for it to complete
                    yield return StartCoroutine(abilities[i].DoAbility(_shape, card));

                    _shape.Clear();
                    break;
                }else if (_skip)
                {
                    ButtonsSetActive(false, false, false);
                    Prompt("", false);
                    CanStopCoroutine(false);
                    HighlightManager.Instance.ClearTargetAndRange();
                    _shape.Clear();
                    break;
                }
                else
                {
                    _shape.Clear();
                    Prompt("", false);
                    ButtonsSetActive(false, false, false); //this is not redudant KEEP IT
                }
                
            }
        }

        //Take away card durability then either destroy it or add it to the discard
        if (card.Rarity != Rarity.BASIC) 
        { 
            card.Durability--;
            if (card.Durability <= 0)
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
        else //card is basic
        {
            //Move from hand to discard
            DeckManager.Instance.HandCardToDiscard(_selectedCardObject);
        }

        //wait until action queue so cant stop coroutine until discards have been done
        yield return new WaitUntil(ActionQueue.Instance.IsQueueStopped);
        _canStopCoroutine = true;
        EndTurnButton.Instance.CanClickEndTurn(true);
    }
    private void Undo()
    {
        //If we can cancel coroutine
        if(!_canStopCoroutine){ return; }

        _canStopCoroutine = false; //can't stop coroutine until move is done

        ClearSpecialPathfind(); //in case there was special pathfind ongoing we clear it up
        _shape.Clear();

        StopCoroutine(_cardLoopCoroutine);
        EndTurnButton.Instance.CanClickEndTurn(true);

        //Get rid of reselect and confirm buttons
        ButtonsSetActive(false, false, false);

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
        TweenManager.Instance.CardMove(_selectedCardObject, cardSlot.position).OnComplete(()=> _canStopCoroutine = true); //can stop coroutine after card move done
        TweenManager.Instance.CardScale(_selectedCardObject, false);

        _selectedCardObject = null; //Unselect card
        SelectedCard = null;
    }

    private bool ButtonsClicked()
    {
        if (_reselect || _confirm || _skip)
        {
            return true;
        }
        return false;
    }

    private bool ShowShape(AbilityBase ability, List<HexNode> range)
    {
        HexNode mouseNode = MouseManager.Instance.NodeMouseIsOver;

        if (!range.Contains(mouseNode)) //If mouse node is within the range then we show the shape
        {
            _priorMouseNode = mouseNode;
            return false;
        }

        if (ability.IsSpecialPathfind)
        {
            if (Input.GetMouseButtonDown(1) && _walkTemp.Contains(mouseNode))
            {
                _walkHex = mouseNode;
                _shape = _shape.Union(_walkTemp).ToList();
            }

            if (mouseNode != _priorMouseNode)
            {
                HandleSpecialPathfind(ability, mouseNode);
            }

            if (_walkTemp.Contains(mouseNode) && NodeClicked())
            {
                _shape = _shape.Union(_walkTemp).ToList();
                _priorMouseNode = mouseNode;
                ClearSpecialPathfind();
                return true;
            }
        }
        else
        {
            if (mouseNode != _priorMouseNode)
            {
                HighlightManager.Instance.ClearTargetMap(); //Clear any prior shape
                _shape = ability.GetShape(mouseNode, SelectedCharacter.GetNodeOn());
                HighlightManager.Instance.HighlightTargetList(_shape);
            }

            if (_shape.Contains(mouseNode) && NodeClicked())
            {
                _priorMouseNode = mouseNode;
                return true;
            }
        }

        _priorMouseNode = mouseNode;
        return false;
    }

    private void ClearSpecialPathfind()
    {
        _walkHex = null;
        _walkShape.Clear();
        _walkTemp.Clear();
    }
    private void HandleSpecialPathfind(AbilityBase ability, HexNode mouseNode)
    {
        HighlightManager.Instance.ClearTargetMap();

        List<HexNode> nodesToRemove = _walkTemp
            .Where(node => _walkShape.Contains(node) && !_shape.Contains(node))
            .ToList();

        foreach (HexNode nodeToRemove in nodesToRemove)
        {
            _walkTemp.Remove(nodeToRemove);
        }

        if (_walkHex == null)
        {
            _walkHex = SelectedCharacter.GetNodeOn();
        }

        _walkShape = ability.GetShape(mouseNode, _walkHex);

        _walkTemp.AddRange(_shape.Union(_walkShape)
            .Where(node => !_walkTemp.Contains(node) && _walkTemp.Count < ability.Range)
            .Where(node => _walkTemp.Count == 0 || HexDistance.GetDistance(_walkTemp.Last(), node) == 1));

        HighlightManager.Instance.HighlightTargetList(_walkTemp);
    }

    private bool CharacterClicked()
    {

        if (NodeClicked() && _clickedNode.GetCharacterOnNode() != null)
        {
            if(_clickedNode.GetCharacterOnNode().IsOwner)
            {
                SelectedCharacter = _clickedNode.GetCharacterOnNode();
                _promptTMP.gameObject.SetActive(false);
                return true;
            }
            else
            {
                PopUp.Instance.PopUpText("You do NOT own this character");
                return false;
            }
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

    private void ButtonsSetActive(bool confirm, bool reselect, bool skip)
    {
        _confirmButton.gameObject.SetActive(confirm);
        _reselectButton.gameObject.SetActive(reselect);
        _skipButton.gameObject.SetActive(skip);
        _confirm = false;
        _reselect = false;
        _skip = false;
    }

    private void AdditivePrompt(string prompt)
    {
        _promptTMP.text += "\n" + prompt;
    }
    private void Prompt(string text, bool setActive)
    {
        _promptTMP.text = text;
        _promptTMP.gameObject.SetActive(setActive);
    }

    /// <summary>
    /// Whether or not the current card ability can undo what it has done
    /// </summary>
    /// <param name="canStop"></param>
    public void CanStopCoroutine(bool canStop)
    {
        _canStopCoroutine = canStop;
        _undoButton.gameObject.SetActive(canStop);
    }
}
