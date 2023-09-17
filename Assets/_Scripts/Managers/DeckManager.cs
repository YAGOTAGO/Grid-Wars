using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TMPro;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    [Header("Deck Nums TMP")]
    [SerializeField] private TextMeshProUGUI _deckNumTMP;
    [SerializeField] private TextMeshProUGUI _discardNumTMP;

    [Header("Prefabs")]
    [SerializeField] private GameObject _cardTemplate;

    [Header("Location Transforms")]
    [SerializeField] private Transform[] _cardSlots; //Keep it to max of 7
    [SerializeField] private Transform _discardTransform;
    public Transform DeckTransform;
    private bool[] _cardSlotsFilled = new bool[7];

    [HideInInspector] public int NumOfCardsDrawn =0;
    #region Lists of cards
    public ObservableCollection<CardBase> _deck = new();
    public ObservableCollection<CardBase> _discard = new();
    public ObservableCollection<CardBase> _hand = new();
    private Dictionary<GameObject, int> _cardPrefabsInHand = new();
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeckDraw(1);
        }

    }

    private void Start()
    {
        // Subscribe to the CollectionChanged event
        _deck.CollectionChanged += OnDeckCollectionChanged;
        _discard.CollectionChanged += OnDiscardCollectionChanged;

        //Add cards to deck here
        AddToDeck(Database.Instance.GetCardByName("FlameBlast"));
        AddToDeck(Database.Instance.GetCardByName("MagicShotgun"));
        AddToDeck(Database.Instance.GetCardByName("Walk"));
        AddToDeck(Database.Instance.GetCardByName("TrapMaking"));
        AddToDeck(Database.Instance.GetCardByName("TidalWave"));



    }
    private void OnDestroy()
    {
        _deck.CollectionChanged -= OnDeckCollectionChanged;
        _discard.CollectionChanged -= OnDiscardCollectionChanged;
    }

    #region Public Methods
    /// <summary>
    /// Chose one deck to add cards to from another place
    /// </summary>
    /// <param name="deck"></param>
    /// <param name="cardsToAdd"></param>
    public void AddToDeck(ObservableCollection<CardBase> deck, ObservableCollection<CardBase> cardsToAdd)
    {
        foreach (CardBase card in cardsToAdd)
        {
            deck.Add(card);
        }
        cardsToAdd.Clear();
    }

    public void AddToDeck(ObservableCollection<CardBase> deck, CardBase card)
    {
        deck.Add(card);
    }
    public void AddToDeck(CardBase card)
    {
        _deck.Add(card);
    }
    public void AddToDiscard(CardBase card)
    {
        _discard.Add(card);
    }

    /// <summary>
    /// Draws if possible from the deck and adds the cards to the hand
    /// </summary>
    /// <param name="drawAmount"></param>
    public void DeckDraw(int drawAmount)
    {
        ActionQueue.Instance.EnqueueMethod(()=> DrawCoroutine(drawAmount));
    }

    public void HandCardToDiscard(GameObject card)
    {
        ActionQueue.Instance.EnqueueMethod(() => DiscardCardFromHandCoroutine(card));
    }

    /// <summary>
    /// Fisher-Yates algorithm to shuffle the deck.
    /// </summary>
    public void ShuffleList(ObservableCollection<CardBase> deck)
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
    /// Given a card game object in hand will return the transform of the slot
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public Transform GetSlotTransformFromCard(GameObject card)
    {
        return _cardSlots[_cardPrefabsInHand[card]];
    }
    #endregion

    #region Big Boi Methods

    private IEnumerator DrawCoroutine(int drawAmount) 
    {

        if (_hand.Count >= 7 || //Hand is full we cannot draw anymore
            (_deck.Count==0 && _discard.Count==0)) //If no cards in deck or discard can't draw
        {
            NumOfCardsDrawn = 0;
            yield break; 
        } 

        if(_deck.Count==0 && _discard.Count > 0) //If deck is empty but have cards in discard
        {
            ShuffleList(_discard); //Shuffle the discard
            AddToDeck(_deck, _discard); //Add the cards in the discard to the deck
        }

        for (int i = drawAmount; i>0 ; i--)
        {
            //if(_deck.Count <= 0) { yield break; }
            //Get and remove card from deck list add to hand list
            CardBase cardDrawn = _deck[0];
            _deck.RemoveAt(0);
            _hand.Add(cardDrawn);

            //Instantiate card display and make it visible in canvas
            Vector3 cardSpawnPosition = new(DeckTransform.position.x-50, DeckTransform.position.y-50);
            GameObject cardDisplay = InstantiateCard(cardDrawn, cardSpawnPosition);
            
            //Find the right slot and Tween it there
            int slotIndex = OpenLeftmostSlotIndex();
            Transform cardSlot = _cardSlots[slotIndex];
            
            cardDisplay.transform.SetParent(cardSlot);
            float cardScale = TweenManager.Instance.CardScaleDown;
            cardDisplay.transform.localScale = new Vector3(cardScale, cardScale, cardScale); //stop any weird behaviour

            _cardSlotsFilled[slotIndex] = true;
            _cardPrefabsInHand[cardDisplay] = slotIndex; //Cache slot index to game object

            Tween cardMove = TweenManager.Instance.CardMove(cardDisplay, cardSlot.position);
            
            //Wait for card draw to finish before moving on
            yield return cardMove.WaitForCompletion();
            
        }

        NumOfCardsDrawn = drawAmount;

        yield return null;
    }

    //Removes the card from all hand related data structs, returns its slot index
    public int RemoveFromHand(GameObject card, bool destroy)
    {
        int indexSlot = _cardPrefabsInHand[card]; //index of slot that card is in
        
        _cardSlotsFilled[indexSlot] = false; //free the bool slot array

        CardBase abstractCard = card.GetComponent<CardDisplay>().GetCard();
        _hand.Remove(abstractCard);
        _cardPrefabsInHand.Remove(card);

        if(destroy) { Destroy(card); }
        return indexSlot;
    }

    private IEnumerator DiscardCardFromHandCoroutine(GameObject card)
    {

        //Find what slot card was in free that slot
        int indexSlot = RemoveFromHand(card, false);
        
        //Update cards in hand and cards in discard
        CardBase abstractCard = card.GetComponent<CardDisplay>().GetCard();
        _discard.Add(abstractCard);

        //Tween card to the discard deck position
        Vector3 discardPosition = new(_discardTransform.position.x+50, _discardTransform.position.y-30);
        Tween cardMove = TweenManager.Instance.CardMove(card, discardPosition).OnComplete(() =>
        {
            Destroy(card);
        });

        yield return cardMove.WaitForCompletion();
    }
    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Uses the card template prefab, instantiates it and updates the display.
    /// </summary>
    /// <param name="card"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject InstantiateCard(CardBase card, Vector3 position)
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
    
    #endregion

    #region Deck/Discard Count TMP updates
    private void UpdateDeckNum()
    {
        _deckNumTMP.text = _deck.Count.ToString();
    }
    private void UpdateDiscardNum()
    {
        _discardNumTMP.text = _discard.Count.ToString();
    }

    // Events for updating the text when deck or discard count changes
    private void OnDeckCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        UpdateDeckNum();
    }
    private void OnDiscardCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        UpdateDiscardNum();
    }
    #endregion

}
