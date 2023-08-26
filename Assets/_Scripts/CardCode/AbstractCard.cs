using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCard
{
    public abstract Rarity Rarity { get; }
    public abstract Sprite CartArt { get; }
    public virtual Sprite ShapeArt { get; } = null;
    public abstract string Description { get; }
    public abstract string Name { get; }
    public abstract int Durability { get; set; }
    public abstract List<AbstractAbility> Abilities { get; }
  

}

