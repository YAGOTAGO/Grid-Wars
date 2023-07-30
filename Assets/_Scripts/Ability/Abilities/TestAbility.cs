using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbility : AbstractAbility
{
    private static int _range;
    private AbstractShape _shape = new LineShape(_range, true, true);
    public override int Range { get => _range; set => _range = value; }

    public override string Prompt => "This is the prompt";

    public override AbstractShape Shape { get => _shape; set => _shape = value; }

    public override void DoAbility(HexNode node)
    {
        Debug.Log("Test Ability Done to: " + node.name);
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
