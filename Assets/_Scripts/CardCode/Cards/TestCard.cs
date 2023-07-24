using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestCard : AbstractCard
{
    #region local vars
    private Sprite _cardArt;
    private Sprite _shapeArt;
    #endregion

    public override int StoreCost => 20;

    public override Sprite CartArt { get => _cardArt; } 

    public override Sprite ShapeArt { get => _shapeArt; }

    public override string Description => "This card;";

    public override string Name => "Testing TEST";

    public override int Range => 3;

    public override void DoAbility(HexNode node)
    {
        throw new System.NotImplementedException();
    }

    public override List<HexNode> GetShape(HexNode mouseNode)
    {
        throw new System.NotImplementedException();
    }
}
