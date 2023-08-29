using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardRewardScreen : MonoBehaviour
{

    public static CardRewardScreen Instance;
    [SerializeField] private List<Transform> _positions = new();
    [SerializeField] private TextMeshProUGUI _promptTmp;

    private void Start()
    {
        Instance = this;
    }
    public void PickThreeCards(Rarity rarity)
    {
        ActionQueue.Instance.EnqueueToFront(()=>CardRewards(rarity));
    }

    private IEnumerator CardRewards(Rarity rarity)
    {

        _promptTmp.gameObject.SetActive(true);
        _promptTmp.text = "Pick 1 of these 3 "+ rarity.ToString() +" cards to add to your deck.";

        //Get the cards we are going to show
        List<CardBase> cards = new();
        switch (rarity)
        {
            case Rarity.COMMON: cards = Database.Instance.GetDifferentCommons(3); break;
            case Rarity.EPIC: cards = Database.Instance.GetDifferentEpics(3); break;
            case Rarity.RARE: cards = Database.Instance.GetDifferentRares(3); break;
        }

        //Instantiate and wait for cards to be clicked
        GameObject card1 = DeckManager.Instance.InstantiateCard(cards[0], _positions[0].position);
        GameObject card2 = DeckManager.Instance.InstantiateCard(cards[1], _positions[1].position);
        GameObject card3 = DeckManager.Instance.InstantiateCard(cards[2], _positions[2].position);

        //Parent the cards to this gameobject to be visible
        card1.transform.SetParent(_positions[0]);
        card2.transform.SetParent(_positions[1]);
        card3.transform.SetParent(_positions[2]);

        //Makes it so the card on click event is for rewards and not actions
        card1.GetComponent<OnCardClick>().CardIsForReward();
        card2.GetComponent<OnCardClick>().CardIsForReward();
        card3.GetComponent<OnCardClick>().CardIsForReward();

        //yield until card is clicked
        yield return new WaitUntil(() =>
        (card1.GetComponent<OnCardClick>().IsCardRewardClicked()) ||
        (card2.GetComponent<OnCardClick>().IsCardRewardClicked()) ||
        (card3.GetComponent<OnCardClick>().IsCardRewardClicked()));

        _promptTmp.gameObject.SetActive(false);

        //Get the card that was clicked
        GameObject chosenCard = null;
        if (card1.GetComponent<OnCardClick>().IsCardRewardClicked()) { chosenCard = card1; Destroy(card2); Destroy(card3); }
        else if (card2.GetComponent<OnCardClick>().IsCardRewardClicked()) { chosenCard = card2; Destroy(card1); Destroy(card3); }
        else if (card3.GetComponent<OnCardClick>().IsCardRewardClicked()) { chosenCard = card3; Destroy(card1); Destroy(card2); }

        //tween card to the deck and then add it to the deck
        Tween cardMove = TweenManager.Instance.CardMove(chosenCard, DeckManager.Instance.DeckTransform.position).OnComplete(() =>
        {
            //Add the card to the deck
            DeckManager.Instance.AddToDeck(chosenCard.GetComponent<CardDisplay>().GetCard());
            Destroy(chosenCard);
        });

        yield return cardMove.WaitForCompletion();
    }

}
