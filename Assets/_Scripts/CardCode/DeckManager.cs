using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;

    [SerializeField] private GameObject _cardTemplate;

    [Header("Location Transforms")]
    [SerializeField] private Transform[] cardSlots; //Keep it to max of 7
    private bool[] cardSlotsFilled = new bool[7];
    [SerializeField] private Transform _deckTransform;

    [Header("ITween Values")]
    [SerializeField] private float _delay = .2f;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private iTween.EaseType _easeType;

    #region Groups of cards
    private List<Card> _deck = new();
    private List<Card> _discard = new();
    private List<Card> _hand = new();
    private List<GameObject> _cardPrefabsInHand = new();
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AddToDeck(_deck, CardDatabase.Instance.GetCardByName(CardName.card1));
        AddToDeck(_deck, CardDatabase.Instance.GetCardByName(CardName.card1)); 
        AddToDeck(_deck, CardDatabase.Instance.GetCardByName(CardName.card1));
        ShuffleDeck();
        DeckToHandDraw();
        DeckToHandDraw();
    }

    /// <summary>
    /// Takes card from deck list (if possible)
    /// </summary>
    public void DeckToHandDraw()
    {
        //
        //ADD IF HAVE HAND SPACE AND CAN"T DRAW AND THERE ARE CARDS IN DISCARD THEN SHUFFLE
        //
        if(_hand.Count >= 7) { return; } //Hand is full we cannot draw anymore

        //Get and remove card from deck add to hand
        Card cardDrawn = _deck[0];
        _deck.RemoveAt(0);
        _hand.Add(cardDrawn);

        //Instantiate card display
        GameObject cardDisplay = InstantiateCard(cardDrawn, _deckTransform.position);
        _cardPrefabsInHand.Add(cardDisplay); //Cache a reference to be able to delete in future
        cardDisplay.transform.SetParent(_deckTransform); //Make it visible in canvas
        cardDisplay.transform.localScale = Vector3.one; //stop any weird behaviour

        StartCoroutine(TweenCardToHand(cardDisplay));
        
    }

    IEnumerator TweenCardToHand(GameObject cardDisplay)
    {
        yield return new WaitForSeconds(_delay); //delay for a bit
        Transform parent = OpenLeftmostSlot();
        iTween.MoveTo(cardDisplay, iTween.Hash("easetype", _easeType, "speed", _speed, "position", parent.position, "localscale", true));
        cardDisplay.transform.SetParent(parent);

    }
      
    public void HandCardToDiscard(GameObject cardDisplay)
    {
        //Find what slot card was in free that slot


        //Tween card to the discard deck position

        //Update cards in hand and cards in discard

        //Destroy the game object

    }


    /// <summary>
    /// Chose one deck to add cards to from another place
    /// </summary>
    /// <param name="deck"></param>
    /// <param name="cardsToAdd"></param>
    public void AddToDeck(List<Card> deck,List<Card> cardsToAdd)
    {
        deck.AddRange(cardsToAdd);
        cardsToAdd.Clear();
    }

    public void AddToDeck(List<Card> deck, Card card)
    {
        deck.Add(card);
    }

    /// <summary>
    /// Fisher-Yates algorithm to shuffle the deck.
    /// </summary>
    public void ShuffleDeck()
    {
        int n = _deck.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            (_deck[n], _deck[k]) = (_deck[k], _deck[n]);
        }
    }

    /// <summary>
    /// Uses the card template prefab, instantiates it and updates the display.
    /// </summary>
    /// <param name="card"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private GameObject InstantiateCard(Card card, Vector3 position)
    {
        //Instatiate the template object
        GameObject cardTemplate = Instantiate(_cardTemplate, position, Quaternion.identity);
        cardTemplate.name = card.name; //Give GameObject a name
        cardTemplate.GetComponent<CardDisplay>().Initialize(card); //Initliaze the display

        return cardTemplate;
    }

    /// <summary>
    /// </summary>
    /// <returns>Returns the slot that is the leftmost open one, and sets that slot to filled</returns>
    private Transform OpenLeftmostSlot()
    {

        for(int i=0; i<cardSlotsFilled.Length; i++)
        {
            if (!cardSlotsFilled[i])
            {
                cardSlotsFilled[i] = true;
                return cardSlots[i];    
            }
        }

        Debug.Log("Couldn't find open leftmost slot called and all slots full.");
        return null;
    }
}
