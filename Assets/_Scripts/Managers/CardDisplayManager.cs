using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplayManager : MonoBehaviour
{
    public static CardDisplayManager Instance;

    [SerializeField] private CardTemplate _cardTemplate;

    private void Start()
    {
        Instance = this;
    }

    //Initialize a card and the group where to put it in
    public void InstantiateCard(Card card, Transform UIGroup)
    {
        //Instatiate the template object
        GameObject cardTemplate = Instantiate(_cardTemplate.template, UIGroup);
        cardTemplate.name = card.name; //Give GameObject a name
        cardTemplate.GetComponent<CardDisplay>().Initialize(card); //Initliaze the display
        
    }

    
}
