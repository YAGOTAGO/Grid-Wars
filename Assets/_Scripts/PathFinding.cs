using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PathFinding
{
   /* //To be able to get the neighbors
    private static readonly Vector3Int[] vectors = new Vector3Int[] {
    new Vector3Int(1, 0),
    new Vector3Int(1, -1),
    new Vector3Int(0, -1),
    new Vector3Int(-1, 0),
    new Vector3Int(-1, 1),
    new Vector3Int(0, 1),
    };

    private readonly List<Vector3Int> _axialNeighbors = new List<Vector3Int>(vectors);
    public static List<HexNode> FindPath(HexNode startNode, HexNode targetNode)
    {
        var toSearch = new List<HexNode>() { startNode };
        var processed = new List<HexNode>();

        while (toSearch.Any())
        {
            var current = toSearch[0];
            foreach (var t in toSearch)
                if (t.F < current.F || t.F == current.F && t.H < current.H) current = t;

            processed.Add(current);
            toSearch.Remove(current);

            //When we find our target
            if (current == targetNode)
            {
                var currentPathTile = targetNode;
                var path = new List<GameRuleTile>();
                var count = 100;
                while (currentPathTile != startNode)
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                    count--;
                    if (count < 0) throw new Exception();
                    Debug.Log("sdfsdf");
                }

                return path;
            }

            foreach (var neighbor in current.Neighboors.Where(t => t.isWalkable && !processed.Contains(t)))
            {
                var inSearch = toSearch.Contains(neighbor);

                //+1 cause distance to neighboor is always 1 on hex grid
                var costToNeighbor = current.G + 1;

                if (!inSearch || costToNeighbor < neighbor.G)
                {
                    neighbor.SetG(costToNeighbor);
                    neighbor.SetConnection(current);

                    if (!inSearch)
                    {
                        neighbor.SetH(neighbor.GetDistance(targetNode));
                        toSearch.Add(neighbor);
                    }
                }
            }
        }
        return null;

    }
    
    public List<HexNode> CacheNeighbors(HexNode hex)
    {
        foreach (Vector3Int vec in _axialNeighbors)
        {

        }
    }

    //Returns a direction
    private Vector3Int AxialDir(int dir)
    {
        return _axialNeighbors[dir];
    }

    //Add dir vec to the hex
    private Vector3Int AxialAdd(Vector3Int hex, Vector3Int vec)
    {
        return new Vector3Int(hex.x + vec.x, hex.y + vec.y);
    }

    //Returns the hex pos with dir added
    private Vector3Int AxialNeighbor(Vector3Int hex, int dir)
    {
        return AxialAdd(hex, AxialDir(dir));
    }*/

}
