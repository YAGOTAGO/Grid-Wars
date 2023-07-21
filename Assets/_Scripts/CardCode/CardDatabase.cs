using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{

    public static CardDatabase Instance { get; private set; }
    public Dictionary<CardName, Card> cardDictionary = new();

    private void Awake()
    {
        Instance = this;
        LoadAllCards();
    }

    /// <summary>
    /// Loads all card scriptable objects into database list
    /// </summary>
    private void LoadAllCards()
    {
        Card[] cardObjects = Resources.LoadAll<Card>("Cards"); // Assuming your ScriptableObjects are in a "Cards" folder within the Resources folder.

        cardDictionary.Clear();
        foreach (Card card in cardObjects)
        {
            // Make sure the card names are unique to prevent overwriting values in the dictionary.
            if (!cardDictionary.ContainsKey(card.CardName))
            {
                cardDictionary.Add(card.CardName, card);
                //Debug.Log("Added: " + card.CardName + " scrip obj: " + card.ToString());
            }
            else
            {
                Debug.LogWarning("Duplicate card name detected: " + card.name +" every scriptable object needs to have a unique name");
            }
        }
    }

    /// <summary>
    /// Given a cardName returns a scriptable oject with that data
    /// </summary>
    /// <param name="cardName"></param>
    /// <returns>A Card Scriptable Object with all data to display a card</returns>
    public Card GetCardByName(CardName cardName)
    {
        if (cardDictionary.TryGetValue(cardName, out Card card))
        {
            return card;
        }
        else
        {
            Debug.LogWarning("Card with the name '" + cardName.ToString() + "' not found.");
            return null;
        }
    }


}
