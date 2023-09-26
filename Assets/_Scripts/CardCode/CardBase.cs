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
    public Class Class;
    [Range(0,10)]
    public int Durability;
    [TextArea(1, 6)]
    public string Description; 
    public List<AbilityBase> Abilities;
    public List<Keyword> Keywords;

}

public enum Rarity
{
    BASIC =0, //Basic cards can't lose durability and start in hand
    COMMON =1, //Frequently found
    RARE = 2, //maybe sometimes in treasure chests
}

public enum Class 
{
    None =0, //Default
    Wizard =1,
    Monk =2,
    Cleric =3,
}

