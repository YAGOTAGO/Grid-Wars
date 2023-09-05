using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardBase : ScriptableObject
{
    public Rarity Rarity;
    public Sprite CardTemplate;
    public Sprite CardArt;
    public Sprite ShapeArt;
    public int Durability;
    public Sprite IconArt; //for the icon on ground pickup
    public string Name;
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
