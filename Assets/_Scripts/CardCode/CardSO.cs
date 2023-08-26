using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardSO : ScriptableObject
{
    public Rarity Rarity;
    public Sprite CardArt;
    public Sprite ShapeArt;
    public string Description;
    public string Name;
    public int Durability;
    public List<AbilityBase> Abilities;
    
}

public enum Rarity
{
    BASIC, //Basic cards can't lose durability and start in hand
    COMMON, //Frequently found
    RARE, //maybe sometimes in treasure chests
    EPIC // maybe unique to events, or certain npcs
}
