using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard : AbstractCard
{
    #region local vars
    private Sprite _cardArt;
    private Sprite _shapeArt;
    private readonly List<AbstractAbility> _abilities = new() { new TestAbility() };
    private int _durability = 2;
    #endregion

    public override int StoreCost => 20;

    public override Sprite CartArt { get => _cardArt; }

    public override Sprite ShapeArt { get => _shapeArt; }

    public override string Description => "This card;";

    public override string Name => "Testing TEST";

    public override int Range => 3;

    public override List<AbstractAbility> Abilities { get => _abilities; }

    public override int Durability { get => _durability; set => _durability = value; }
}
