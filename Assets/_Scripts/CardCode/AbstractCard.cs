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
    #endregion

    #region Methods
    /// <summary>
    /// Use methods in Shape script to return a shape
    /// </summary>
    /// <param name="mouseNode">The node which we want shape to be based around</param>
    /// <returns>List of HexNode based on given node</returns>
    public abstract List<HexNode> GetShape(HexNode mouseNode);

    /// <summary>
    /// Does the ability of the card to the given node
    /// </summary>
    /// <param name="affectedNode">Node to do ability on</param>
    public abstract void DoAbility(HexNode node);
    #endregion

}
