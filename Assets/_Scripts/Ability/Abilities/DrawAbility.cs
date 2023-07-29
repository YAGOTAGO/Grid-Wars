using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAbility : AbstractAbility
{
    private readonly int _drawAmount;

    public override string Prompt => "Draw " + _drawAmount + " cards.";
    public override void DoAbility(HexNode node)
    {
        DeckManager.Instance.DeckDraw(_drawAmount);
    }

    public override TargetingType GetTargetingType()
    {
        return TargetingType.NONE;
    }

    public DrawAbility(int drawAmount)
    {
        _drawAmount = drawAmount;
    }
}
