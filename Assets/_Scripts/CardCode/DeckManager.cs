using DG.Tweening;
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
    [SerializeField] private Transform _canvas;

    [Header("Tween Values")]
    [SerializeField, Range(0,20)] private float _duration = 1f;
    [SerializeField, Range(0,2)] private float _drawDelay = 1f;
    [SerializeField] private Ease _ease;

    #region Groups of cards
    private List<AbstractCard> _deck = new();
    private List<AbstractCard> _discard = new();
    private List<AbstractCard> _hand = new();
    private List<GameObject> _cardPrefabsInHand = new();
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AddToDeck(_deck, new TestCard());
        AddToDeck(_deck, new TestCard());
        AddToDeck(_deck, new TestCard());
        ShuffleDeck();
        DeckDraw(2);
    
    }

    /// <summary>
    /// Draws if possible from the deck and adds the cards to the hand
    /// </summary>
    /// <param name="drawAmount"></param>
    public void DeckDraw(int drawAmount)
    {
        StartCoroutine(DeckToHandDraw(drawAmount));
    }

   
    private IEnumerator DeckToHandDraw(int drawAmount) //try and tweak later to make it feel better
    {
        for (int i = 0; i < drawAmount; i++)
        {
            //
            //ADD IF HAVE HAND SPACE AND CAN"T DRAW AND THERE ARE CARDS IN DISCARD THEN SHUFFLE
            //
            if (_hand.Count >= 7) { yield return null; } //Hand is full we cannot draw anymore

            //Get and remove card from deck add to hand
            AbstractCard cardDrawn = _deck[0];
            _deck.RemoveAt(0);
            _hand.Add(cardDrawn);

            //Instantiate card display
            GameObject cardDisplay = InstantiateCard(cardDrawn, _deckTransform.position);
            _cardPrefabsInHand.Add(cardDisplay); //Cache a reference to be able to delete in future

            cardDisplay.transform.SetParent(_canvas); //Make it visible in canvas
            cardDisplay.transform.localScale = Vector3.one; //stop any weird behaviour

            //Find the right slot and move it there
            Transform parent = OpenLeftmostSlot(cardDisplay.transform);
            cardDisplay.transform.DOMove(parent.position, _duration).SetEase(_ease);

            if (drawAmount > 1)
            {
                yield return new WaitForSeconds(_drawDelay);
            }
            
        }

        yield return null;
    }

      
    public void HandCardToDiscard(GameObject card)
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
    /// <returns>Returns the slot that is the leftmost open one, and sets that slot to filled</returns>
    private Transform OpenLeftmostSlot(Transform cardTransform)
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
