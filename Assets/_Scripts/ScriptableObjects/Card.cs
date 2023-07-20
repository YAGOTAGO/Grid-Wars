using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
   
    public AbstractAbility Ability; //Ability that card will do
    public int StoreCost;
    public Sprite CardArt;
    public string Description;
    public int Range;
    public Sprite Shape;
    public CardName CardName;
}
