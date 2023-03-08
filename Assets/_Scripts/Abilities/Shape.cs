using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    private Vector3Int _up = new(1,0);
    private Vector3Int _down = new(-1, 0);
    private Vector3Int _upRight = new(0, 1);
    private Vector3Int _downRight = new(-1, 1);
    private Vector3Int _upLeft = new(0, -1);
    private Vector3Int _downLeft = new(-1,-1);

    public PlayerMovement p;
    public List<HexNode> nodes;

    #region prior vars
    private HexNode _priorNode;
    private List<HexNode> _priorShape;
    #endregion

    private void Update()
    {
        /*nodes = LineInDirection(p._onNode, GridManager.Instance.GridCoordTiles[MouseManager.Instance.MouseCellPos], 3);

        foreach (HexNode node in nodes)
        {
            HighlightManager.Instance.PathHighlight(node.GridPos);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HighlightManager.Instance.ClearPathMap();
        }*/
        
    }

    /// <summary>
    /// A amount size line that doesn't include the player, and will not pass terrain
    /// </summary>
    /// <param name="playerNode">Node player is at</param>
    /// <param name="mouseNode">Node mouse is at</param>
    /// <param name="amount">How long the straight line should be</param>
    /// <returns>A list of nodes of the shape</returns>
    public List<HexNode> LineInDirection(HexNode playerNode, HexNode mouseNode, int amount)
    {

        //Only call if mouse moved node
        if(_priorNode == mouseNode){return _priorShape;}

        //clear path
        HighlightManager.Instance.ClearPathMap();
        _priorNode = mouseNode;

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

            if(GridManager.Instance.CubeCoordTiles.TryGetValue(currNode.CubeCoord + directionInt, out HexNode Value))
            {
                currNode = Value;
            }
            
            if(currNode != null && currNode != playerNode && currNode.IsPassable)
            {
                nodesInDirection.Add(currNode);
            }
            else
            {
                _priorShape = nodesInDirection;
                return nodesInDirection;
            }

        }

        _priorShape = nodesInDirection;
        return nodesInDirection;
    }

    public List<HexNode> Circle (HexNode origin, bool includeOrigin)
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
