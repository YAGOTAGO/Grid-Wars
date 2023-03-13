using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbility : AbstractAbility
{
    public int Cooldown = 1;
    public int Range = 3;

    public override int GetRange(){ return Range; }

    public override bool ShouldDisplayRange()
    {
        return false;
    }

    public override void DoAbility(HexNode node)
    {
        Debug.Log("Test Ability Done to: " + node.name);
    }
       
    public override List<HexNode> GetShape(HexNode mouseNode)
    {
        return Shape.LineInDirection(GetHexPlayerIsOn() , mouseNode, Range);
    }

    
}
