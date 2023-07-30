using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCard
{
    #region Stats
    public abstract Rarity Rarity { get; }
    public abstract Sprite CartArt { get; }
    public virtual Sprite ShapeArt { get; } = null;
    public abstract string Description { get; }
    public abstract string Name { get; }
    public abstract int Durability { get; set; }
    public abstract List<AbstractAbility> Abilities { get; }
   
    #endregion
    

}

public enum Rarity
{
    BASIC, //Basic cards can't lose durability and start in hand
    COMMON, //Frequently found
    RARE, //maybe sometimes in treasure chests
    EPIC // maybe unique to events, or certain npcs
}
