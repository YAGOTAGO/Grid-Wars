using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineShape : AbstractShape
{
    public override List<HexNode> GetShape(HexNode mouseNode, AbilityBase ability)
    {
        //Cubic coords
        List<HexNode> nodesInDirection = new();
        HexNode playerNode = CardSelectionManager.Instance.SelectedCharacter.GetNodeOn();
        
        Vector3Int playerCubeCoord = playerNode.CubeCoord.Value;//start
        Vector3Int mouseCubeCoord = mouseNode.CubeCoord.Value; //target

        //Displacements, and distance
        Vector3 displacement = mouseCubeCoord - playerCubeCoord;
        Vector3 direction = displacement.normalized;
        Vector3Int directionInt = new(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y), Mathf.RoundToInt(direction.z));

        TargetingType targetingType = ability.GetTargetingType();
        int range = ability.Range;

        // Mouse is on the same node as player, return empty list
        if (displacement == Vector3Int.zero) { return nodesInDirection; }

        HexNode currNode = playerNode;
        for (int i = 0; i < range; i++)
        {
            if (GridManager.Instance.CubeCoordTiles.TryGetValue(currNode.CubeCoord.Value + directionInt, out HexNode nextNode))
            {
                currNode = nextNode;

                if (targetingType == TargetingType.NORMAL && !currNode.CanAbilitiesPassthrough()) //if type is normal and node cannot be passthrough then we do not add it
                {
                    break;
                }

                if(targetingType == TargetingType.WALKABLE && !currNode.IsNodeWalkable()) //if walkable and node is not walkable then we dont add
                {
                    break;
                }

                nodesInDirection.Add(currNode); //if aireal targeting then always add
            }
            else
            {
                break;
            }

        }

        return nodesInDirection;
    }

    public override List<HexNode> Range(HexNode startNode, AbilityBase ability)
    {
        List<HexNode> nodesRange = new();

        foreach (Vector3Int side in cuberCoordSides)
        {
            if (GridManager.Instance.CubeCoordTiles.TryGetValue(startNode.CubeCoord.Value + side, out HexNode outputNode))
            {
                nodesRange.AddRange(GetShape(outputNode, ability));
            }
        }

        return nodesRange;
    }
}
