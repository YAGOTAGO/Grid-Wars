using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleShape : AbstractShape
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

        bool isTargNormal = ability.GetTargetingType() == TargetingType.NORMAL;
        int range = ability.Range;

        // Mouse is on the same node as player, return empty list
        if (displacement == Vector3Int.zero) { return nodesInDirection; }

        HexNode currNode = playerNode;

        //Foward direction
        for (int i = 0; i < range; i++)
        {
            if (GridManager.Instance.CubeCoordTiles.TryGetValue(currNode.CubeCoord.Value + directionInt, out HexNode nextNode))
            {
                currNode = nextNode;

                if (isTargNormal && !currNode.CanAbilitiesPassthrough()) //if type is normal and node cannot be passthrough then we do not add it
                {
                    break;
                }

                nodesInDirection.Add(currNode);
            }
            else
            {
                break;
            }

        }
        
        currNode = playerNode;
        //Bottom right direction
        for (int i = 0; i < range; i++)
        {
            if (GridManager.Instance.CubeCoordTiles.TryGetValue(currNode.CubeCoord.Value + new Vector3Int(directionInt.y, directionInt.z, directionInt.x), out HexNode nextNode))
            {
                currNode = nextNode;

                if (isTargNormal && !currNode.CanAbilitiesPassthrough()) //if type is normal and node cannot be passthrough then we do not add it
                {
                    break;
                }

                nodesInDirection.Add(currNode);
            }
            else
            {
                break;
            }

        }

        currNode = playerNode;
        //Bottom left direction
        for (int i = 0; i < range; i++)
        {
            if (GridManager.Instance.CubeCoordTiles.TryGetValue(currNode.CubeCoord.Value + new Vector3Int(directionInt.z, directionInt.x, directionInt.y), out HexNode nextNode))
            {
                currNode = nextNode;

                if (isTargNormal && !currNode.CanAbilitiesPassthrough()) //if type is normal and node cannot be passthrough then we do not add it
                {
                    break;
                }

                nodesInDirection.Add(currNode);
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
        return new LineShape().Range(startNode, ability);
    }
}
