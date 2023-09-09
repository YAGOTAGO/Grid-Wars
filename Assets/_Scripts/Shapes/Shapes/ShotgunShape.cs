using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShape : AbstractShape
{
    public override List<HexNode> GetShape(HexNode mouseNode, AbilityBase ability)
    {
        LineShape line = new();

        List<HexNode> lineShape = line.GetShape(mouseNode, ability);
        List<HexNode> shape = new();

        HexNode selectedCharacterNode = CardSelectionManager.Instance.SelectedCharacter.GetNodeOn();

        // Calculate the direction from the selected character's node to the target character's node.
        Vector3 displacement = mouseNode.CubeCoord.Value - selectedCharacterNode.CubeCoord.Value;
        Vector3 direction = displacement.normalized;
        Vector3Int directionInt = new(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y), Mathf.RoundToInt(direction.z));

        foreach (HexNode node in lineShape)
        {
            //if no character on there then add it and keep going
            if(node.GetCharacterOnNode() == null)
            {
                shape.Add(node); //otherwise add it and get 3 tiles behind and break
            }
            else
            {
                shape.Add(node);
                Vector3Int cubeCoords = node.CubeCoord.Value;

                if(GridManager.Instance.CubeCoordTiles.TryGetValue(cubeCoords + directionInt, out HexNode behindNode))
                    shape.Add(behindNode);
                if(GridManager.Instance.CubeCoordTiles.TryGetValue(cubeCoords + new Vector3Int(-directionInt.y, directionInt.x + directionInt.y, -directionInt.x), out HexNode leftNode))
                    shape.Add(leftNode);
                if (GridManager.Instance.CubeCoordTiles.TryGetValue(cubeCoords + new Vector3Int(directionInt.y + directionInt.x, directionInt.y + directionInt.z, directionInt.x + directionInt.z), out HexNode rightNode))
                    shape.Add(rightNode);
                break;
            }

        }

        return shape;
        
    }

}
