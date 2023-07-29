using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCard
{
    #region Stats
    public abstract int StoreCost { get; }
    public abstract Sprite CartArt { get; }
    public abstract Sprite ShapeArt { get; }
    public abstract string Description { get; }
    public abstract string Name { get; }
    public abstract int Range { get; }
    public abstract int Durability { get; set; }
    public abstract List<AbstractAbility> Abilities { get; }

    #endregion
    
    

}
