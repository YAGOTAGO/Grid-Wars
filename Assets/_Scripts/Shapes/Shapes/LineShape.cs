using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineShape : AbstractShape
{
    private int _range;
    private bool _isTargTypeNormal;

    public override List<HexNode> GetShape(HexNode mouseNode)
    {
        //Cubic coords
        List<HexNode> nodesInDirection = new();
        HexNode playerNode = CardSelectionManager.Instance.ClickedCharacter.NodeOn;
        
        Vector3Int playerCubeCoord = playerNode.CubeCoord;//start
        Vector3Int mouseCubeCoord = mouseNode.CubeCoord; //target

        //Displacements, and distance
        Vector3 displacement = mouseCubeCoord - playerCubeCoord;
        Vector3 direction = displacement.normalized;
        Vector3Int directionInt = new(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y), Mathf.RoundToInt(direction.z));

        // Mouse is on the same node as player, return empty list
        if (displacement == Vector3Int.zero) { return nodesInDirection; }

        HexNode currNode = playerNode;
        for (int i = 0; i < _range; i++)
        {
            if (GridManager.Instance.CubeCoordTiles.TryGetValue(currNode.CubeCoord + directionInt, out HexNode nextNode))
            {
                currNode = nextNode;

                if (_isTargTypeNormal && !currNode.CanAbilitiesPassthrough()) //if type is normal and node cannot be passthrough then we do not add it
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

    public LineShape(int range, bool isTypeNormal)
    {
        _range = range;
        _isTargTypeNormal = isTypeNormal;
    }
}
