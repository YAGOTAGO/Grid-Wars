using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindShape : AbstractShape
{
    public override List<HexNode> GetShape(HexNode mouseNode, AbstractAbility ability)
    {
        return PathFinding.FindPathBreakpoints(CardSelectionManager.Instance.ClickedCharacter.NodeOn, mouseNode, CardSelectionManager.Instance.BreakPoints);
    }
}
