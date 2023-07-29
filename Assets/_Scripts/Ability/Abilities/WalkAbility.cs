using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAbility : AbstractAbility
{
    private int _range;
    public override int Range { get => _range; set => _range = value; }

    public override void DoAbility(HexNode node)
    {
        Character character = CardSelectionManager.Instance.ClickedCharacter;

    }

    public override List<HexNode> GetShape(HexNode mouseNode)
    {
        return PathFinding.FindPath(CardSelectionManager.Instance.ClickedCharacter.NodeOn, mouseNode);
    }

    public override TargetingType GetTargetingType()
    {
        return TargetingType.WALKABLE;
    }

    public WalkAbility(int range)
    {
        Range = range;
    }
}
