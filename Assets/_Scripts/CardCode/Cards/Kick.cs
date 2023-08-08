using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : AbstractCard
{
    private int _durability = int.MaxValue; //infinite durability
    private static readonly int _damage = 2;
    private readonly string _prompt = "Deal " + _damage + " to an adjacent character";
    private readonly TargetingType _targetingType = TargetingType.NORMAL;
    private readonly DamageType _dmgType = DamageType.Normal;
    private readonly int _range = 1;

    public override Sprite CartArt => null;
    public override Rarity Rarity => Rarity.BASIC;
    public override string Description => "Deal " + _damage + " to an adjacent character.";
    public override string Name => "Kick";

    public override int Durability { get => _durability; set => _durability = value; }

    public override List<AbstractAbility> Abilities => new() {
        new DrawAbility(1),
        new DamageAbility(_range, new SingleHexShape(), new DamageInfo(_damage, _dmgType, null, null), _targetingType, _prompt)
    };

    
}
