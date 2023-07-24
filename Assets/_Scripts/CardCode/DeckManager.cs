using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    [Header("Deck Nums")]
    [SerializeField] private TextMeshProUGUI _deckNumTMP;
    [SerializeField] private TextMeshProUGUI _discardNumTMP;

    [Header("Prefabs")]
    [SerializeField] private GameObject _cardTemplate;

    [Header("Location Transforms")]
    [SerializeField] private Transform[] _cardSlots; //Keep it to max of 7
    private bool[] _cardSlotsFilled = new bool[7];
    [SerializeField] private Transform _deckTransform;
    [SerializeField] private Transform _discardTransform;
    [SerializeField] private Transform _canvas;

    [Header("Tween Values")]
    [SerializeField, Range(0,20)] private float _tweenDuration;
    [SerializeField, Range(0,2)] private float _actionsDelay;
    [SerializeField] private Ease _ease;

    #region lists of cards
    public List<AbstractCard> _deck = new();
    public List<AbstractCard> _discard = new();
    public List<AbstractCard> _hand = new();
    private Dictionary<GameObject, int> _cardPrefabsInHand = new();
    #endregion

    #region Coroutine Queue
    private Queue<Func<IEnumerator>> methodQueue = new();
    private bool isQueueRunning = false;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandCardToDiscard(_cardPrefabsInHand.Keys.First());
        }
    }
    private void Start()
    {
        AddToDeck(_deck, new TestCard());
        DeckDraw(1);
        _deck.Add(new TestCard());
        _deck.Add(new TestCard());
        ShuffleList(_deck);
         
    }

    /// <summary>
    /// Draws if possible from the deck and adds the cards to the hand
    /// </summary>
    /// <param name="drawAmount"></param>
    public void DeckDraw(int drawAmount)
    {
        EnqueueMethod(()=> DrawCoroutine(drawAmount));
    }

    public void HandCardToDiscard(GameObject card)
    {
        EnqueueMethod(() => DiscardCardFromHandCoroutine(card));
    }
   
    private IEnumerator DrawCoroutine(int drawAmount) 
    {

        if (_hand.Count >= 7 || //Hand is full we cannot draw anymore
            (_deck.Count==0 && _discard.Count==0)) //If no cards in deck or discard can't draw
        { 
            yield return null; 
        } 

        if(_deck.Count==0 && _discard.Count > 0) //If deck is empty but have cards in discard
        {
            ShuffleList(_discard); //Shuffle the discard
            AddToDeck(_deck, _discard); //Add the cards in the discard to the deck
        }

        for (int i = drawAmount; i>0 ; i--)
        {

            //Get and remove card from deck list add to hand list
            AbstractCard cardDrawn = _deck[0];
            _deck.RemoveAt(0);
            _hand.Add(cardDrawn);

            //Instantiate card display and make it visible in canvas
            GameObject cardDisplay = InstantiateCard(cardDrawn, _deckTransform.position);
            cardDisplay.transform.SetParent(_canvas); 
            cardDisplay.transform.localScale = Vector3.one; //stop any weird behaviour

            //Find the right slot and Tween it there
            int slotIndex = OpenLeftmostSlotIndex();
            Transform cardSlot = _cardSlots[slotIndex];
            _cardSlotsFilled[slotIndex] = true;
            _cardPrefabsInHand[cardDisplay] = slotIndex;

            cardDisplay.transform.DOMove(cardSlot.position, _tweenDuration).SetEase(_ease);
            
            //Adds a delay if more cards to draw
            if (i > 1) { yield return new WaitForSeconds(_actionsDelay); }
            
        }

        yield return null;
    }

    private IEnumerator DiscardCardFromHandCoroutine(GameObject card)
    {
        //Find what slot card was in free that slot
        int indexSlot = _cardPrefabsInHand[card];
        _cardSlotsFilled[indexSlot] = false; //free the bool slot array
        Transform cardSlot = _cardSlots[indexSlot];
        
        //Update cards in hand and cards in discard
        AbstractCard abstractCard = card.GetComponent<CardDisplay>().GetCard();
        _hand.Remove(abstractCard);
        _discard.Add(abstractCard);
        _cardPrefabsInHand.Remove(card);

        //Tween card to the discard deck position
        card.transform.DOMove(_discardTransform.position, _tweenDuration).SetEase(_ease).OnComplete(() =>
        {
            Destroy(card);
        });

        yield return new WaitForSeconds(_actionsDelay);
    }

    /// <summary>
    /// Chose one deck to add cards to from another place
    /// </summary>
    /// <param name="deck"></param>
    /// <param name="cardsToAdd"></param>
    public void AddToDeck(List<AbstractCard> deck,List<AbstractCard> cardsToAdd)
    {
        deck.AddRange(cardsToAdd);
        cardsToAdd.Clear();
    }

    public void AddToDeck(List<AbstractCard> deck, AbstractCard card)
    {
        
        deck.Add(card);
    }

    /// <summary>
    /// Fisher-Yates algorithm to shuffle the deck.
    /// </summary>
    public void ShuffleList(List<AbstractCard> deck)
    {
        int n = deck.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            (deck[n], deck[k]) = (deck[k], deck[n]);
        }
    }

    /// <summary>
    /// Uses the card template prefab, instantiates it and updates the display.
    /// </summary>
    /// <param name="card"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private GameObject InstantiateCard(AbstractCard card, Vector3 position)
    {
        //Instatiate the template object
        GameObject cardTemplate = Instantiate(_cardTemplate, position, Quaternion.identity);
        cardTemplate.name = card.Name; //Give GameObject a name
        cardTemplate.GetComponent<CardDisplay>().Initialize(card); //Initliaze the display

        return cardTemplate;
    }

    /// <summary>
    /// </summary>
    /// <returns>Returns the index that is the leftmost open one, and sets that slot to filled</returns>
    private int OpenLeftmostSlotIndex()
    {

        for(int i=0; i<_cardSlotsFilled.Length; i++)
        {
            if (!_cardSlotsFilled[i])
            {
                return i;    
            }
        }

        Debug.Log("Couldn't find open leftmost slot called and all slots full.");
        return -1;
    }
    
    
    /// <summary>
    /// Used to do coroutines in a list sequentially
    /// </summary>
    /// <param name="method">EnqueueMethod(()=> yourmethodhere(param))</param>
    private void EnqueueMethod(Func<IEnumerator> method)
    {
        methodQueue.Enqueue(method);

        if (!isQueueRunning) { StartCoroutine(ProcessQueue()); }
    }

    // Coroutine to process the method queue
    private IEnumerator ProcessQueue()
    {
        isQueueRunning = true;

        while (methodQueue.Count > 0)
        {
            Func<IEnumerator> method = methodQueue.Dequeue();
            yield return StartCoroutine(method());
        }

        isQueueRunning = false;
    }

    private void UpdateDeckNum()
    {
        _deckNumTMP.text = _deck.Count.ToString();
    }
    private void UpdateDiscardNum()
    {
        _discardNumTMP.text = _discard.Count.ToString();
    }
}
