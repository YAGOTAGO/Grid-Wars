using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard : AbstractCard
{
    #region local vars
    private Sprite _cardArt;
    private Sprite _shapeArt;
    private readonly List<AbstractAbility> _abilities = new() { new DrawAbility(2), new WalkAbility(3), new TestAbility(3) };
    private int _durability = 2;
    #endregion

    public override Sprite CartArt { get => _cardArt; }

    public override Sprite ShapeArt { get => _shapeArt; }

    public override string Description => "This card;";

    public override string Name => "Testing TEST";

    public override List<AbstractAbility> Abilities { get => _abilities; }

    public override int Durability { get => _durability; set => _durability = value; }

    public override Rarity Rarity => Rarity.BASIC;
}
