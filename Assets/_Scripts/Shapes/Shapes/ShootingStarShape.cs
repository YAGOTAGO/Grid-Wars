using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStarShape : AbstractShape
{
    readonly LineShape lineShape = new();
    public override List<HexNode> GetShape(HexNode mouseNode, HexNode startingNode, AbilityBase ability)
    {
        List<HexNode> shape = lineShape.GetShape(mouseNode, startingNode, ability);

        // Calculate the direction from the selected character's node to the target character's node.
        Vector3 displacement = mouseNode.CubeCoord.Value - startingNode.CubeCoord.Value;
        Vector3 direction = displacement.normalized;
        Vector3Int directionInt = new(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y), Mathf.RoundToInt(direction.z));

        if(shape.Count == 0) { return shape; } //if no line shape then no shooting star end

        HexNode currNode = shape[^1]; //last node in line

        if (GridManager.Instance.CubeCoordTiles.TryGetValue(currNode.CubeCoord.Value + directionInt, out HexNode behindNode))
            shape.Add(behindNode);

        if(behindNode == null) { return shape; }

        if (GridManager.Instance.CubeCoordTiles.TryGetValue(behindNode.CubeCoord.Value + new Vector3Int(directionInt.x + directionInt.y, directionInt.y + directionInt.z, directionInt.x + directionInt.z), out HexNode leftNode))
            shape.Add(leftNode);
        if (GridManager.Instance.CubeCoordTiles.TryGetValue(behindNode.CubeCoord.Value + new Vector3Int(directionInt.x + directionInt.z, directionInt.x + directionInt.y, directionInt.y + directionInt.z), out HexNode rightNode))
            shape.Add(rightNode);

        return shape;
    }

    public override List<HexNode> Range(HexNode startNode, AbilityBase ability)
    {
        return lineShape.Range(startNode, ability);
    }
}
