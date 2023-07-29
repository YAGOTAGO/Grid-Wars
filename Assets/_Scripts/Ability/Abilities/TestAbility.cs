using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbility : AbstractAbility
{
    private int _range;
    public override int Range { get => _range; set => _range = value; }

    public override void DoAbility(HexNode node)
    {
        Debug.Log("Test Ability Done to: " + node.name);
    }
       
    public override List<HexNode> GetShape(HexNode mouseNode)
    {
        return Shape.LineInDirection(CardSelectionManager.Instance.ClickedCharacter.NodeOn , mouseNode, Range, true);
    }

    public override TargetingType GetTargetingType()
    {
        return TargetingType.NORMAL;
    }

    public TestAbility(int range)
    {
        _range = range;
    }
}
