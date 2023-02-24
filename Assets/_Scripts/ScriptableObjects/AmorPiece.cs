using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Armor Piece")]
public class AmorPiece : ScriptableObject
{
    public ArmorType armorType;
    public ArmorCollection armorCollection;
    public int requiredLevel;
    public int value;
    public Sprite sprite;


}
