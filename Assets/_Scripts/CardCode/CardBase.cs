using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardBase : ScriptableObject
{
    [Header("Art")]
    public Sprite CardArt;
    public Sprite IconArt; //for the icon on ground pickup
    public Sprite ShapeArt;

    [Header("Info")]
    public string Name;
    public Rarity Rarity;
    public int Durability;
    public string Description; 
    public List<AbilityBase> Abilities;
    public List<Keyword> Keywords;
}

public enum Rarity
{
    BASIC, //Basic cards can't lose durability and start in hand
    COMMON, //Frequently found
    RARE, //maybe sometimes in treasure chests
    EPIC // maybe unique to events, or certain npcs
}
