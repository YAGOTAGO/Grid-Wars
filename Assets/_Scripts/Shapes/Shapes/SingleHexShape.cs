using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHexShape : AbstractShape
{
    public override List<HexNode> GetShape(HexNode mouseNode, AbstractAbility ability)
    {
        return new List<HexNode>() { mouseNode };
    }
}
