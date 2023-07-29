using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    private Vector3Int _up = new(1,0);
    private Vector3Int _down = new(-1, 0);
    private Vector3Int _upRight = new(0, 1);
    private Vector3Int _downRight = new(-1, 1);
    private Vector3Int _upLeft = new(0, -1);
    private Vector3Int _downLeft = new(-1,-1);

    /// <summary>
    /// A amount size line that doesn't include the player, and will not pass terrain
    /// </summary>
    /// <param name="playerNode">Node player is at</param>
    /// <param name="mouseNode">Node mouse is at</param>
    /// <param name="amount">How long the straight line should be</param>
    /// <returns>A list of nodes of the shape</returns>
    public static List<HexNode> LineInDirection(HexNode playerNode, HexNode mouseNode, int amount, bool typeNormal)
    {

        //Cubic coords
        List<HexNode> nodesInDirection = new();
        Vector3Int playerCubeCoord = playerNode.CubeCoord;
        Vector3Int mouseCubeCoord = mouseNode.CubeCoord;
        
        //Displacements, and distance
        Vector3 displacement = mouseCubeCoord - playerCubeCoord;
        Vector3 direction = displacement.normalized;
        Vector3Int directionInt = new(Mathf.RoundToInt(direction.x), Mathf.RoundToInt(direction.y), Mathf.RoundToInt(direction.z));

        // Mouse is on the same node as player, return empty list
        if (displacement == Vector3Int.zero){ return nodesInDirection;}

        HexNode currNode = playerNode;
        for(int i = 0; i < amount; i++)
        {
            if (GridManager.Instance.CubeCoordTiles.TryGetValue(currNode.CubeCoord + directionInt, out HexNode nextNode))
            {
                currNode = nextNode;

                if (typeNormal && !currNode.CanAbilitiesPassthrough())
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

    public static List<HexNode> Circle (HexNode origin, bool includeOrigin)
    {
        if (includeOrigin)
        {
            List<HexNode> circle = new(origin.Neighboors){origin};
            return circle;
        }
        else
        {
            return origin.Neighboors;
        }
    }
    
}
