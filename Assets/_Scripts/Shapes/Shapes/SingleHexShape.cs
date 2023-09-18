using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleHexShape : AbstractShape
{
    public override List<HexNode> GetShape(HexNode mouseNode, AbilityBase ability)
    {
        return new List<HexNode>() { mouseNode };
    }

    public override List<HexNode> Range(HexNode startNode, AbilityBase ability)
    {
        return BFS.TargTypeBFS(startNode, ability);
    }
}
