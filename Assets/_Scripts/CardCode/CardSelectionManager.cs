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
    [SerializeField] private Button _skipButton;
    [SerializeField] private Button _undoButton;
    private bool _reselect;
    private bool _confirm;
    private bool _skip;

    #region Card Ability Loop
    private GameObject _selectedCardObject; //game object so we can potentially destroy card
    private CardBase _selectedCard; //The information of what the card does
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
        
        ButtonsSetActive(false, false, false); //no buttons are active

        //Add listeners so buttons do things
        _confirmButton.onClick.AddListener(() => _confirm = true);
        _reselectButton.onClick.AddListener(() => _reselect = true);
        _skipButton.onClick.AddListener(() => _skip = true);

    }

    public void OnClickCard(GameObject card)
    {
        if (!_canStopCoroutine) { return; } //Stops selecting a new card after an action has taken place

        card.GetComponent<CardDisplay>().DisplayKeyword(false); //Selected card removes keyword display

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

    private IEnumerator CardAbilityLoop(CardBase card)
    {
        Prompt("Select a Character", true);
        yield return new WaitUntil(() => CharacterClicked()); //wait until character is selected

        List<AbilityBase> abilities = card.Abilities;
        HashSet<HexNode> range = new();

        for (int i = 0; i < abilities.Count; i++) //iterate over all abilities in the card
        {

            yield return new WaitUntil(ActionQueue.Instance.IsQueueStopped); //Before starting any ability we wait for action queue

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
                        ButtonsSetActive(true, false, true); //we can confirm or skip
                        yield return new WaitUntil(() => ButtonsClicked());
                        if (_skip) //if skip we dont do ability
                        {
                            ButtonsSetActive(false, false, false);
                            Prompt("", false);
                            continue;
                        }

                        abilities[i].DoAbility(new List<HexNode> { ClickedCharacter.NodeOn }); //do ability if not skip
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
                    abilities[i].DoAbility(new List<HexNode> { ClickedCharacter.NodeOn }); //Pass the node character is on
                    CanStopCoroutine(false);
                    ButtonsSetActive(false, false, false);
                    Prompt("", false);
                    continue;
            }

            HighlightManager.Instance.HighlightRangeSet(range);

            yield return null; //have to wait a frame before trying to detect click again

            //loops if click reselect button
            while (true)
            {
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
                    yield return StartCoroutine(abilities[i].DoAbility(_shape));
                    
                    break;
                }else if (_skip)
                {
                    ButtonsSetActive(false, false, false);
                    Prompt("", false);
                    CanStopCoroutine(false);
                    HighlightManager.Instance.ClearTargetAndRange();
                    break;
                }
                else
                {
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

        //This is after cards abilities
        _canStopCoroutine = true;

    }
    private void Undo()
    {
        //If we can cancel coroutine
        if(!_canStopCoroutine){ return; }

        StopCoroutine(_cardLoopCoroutine);

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
        TweenManager.Instance.CardMove(_selectedCardObject, cardSlot.position);
        TweenManager.Instance.CardScale(_selectedCardObject, 1f);

        _selectedCardObject = null; //Unselect card
        _selectedCard = null;
    }

    private bool ButtonsClicked()
    {
        if (_reselect || _confirm || _skip)
        {
            return true;
        }
        return false;
    }

    private bool ShowShape(AbilityBase ability, HashSet<HexNode> range)
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

    private void ButtonsSetActive(bool confirm, bool reselect, bool skip)
    {
        _confirmButton.gameObject.SetActive(confirm);
        _reselectButton.gameObject.SetActive(reselect);
        _skipButton.gameObject.SetActive(skip);
        _confirm = false;
        _reselect = false;
        _skip = false;
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
