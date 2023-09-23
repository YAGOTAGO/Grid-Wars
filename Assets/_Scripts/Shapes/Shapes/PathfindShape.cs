using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfindShape : AbstractShape
{
    public override List<HexNode> GetShape(HexNode targetNode, HexNode startNode, AbilityBase ability)
    {

        List<HexNode> toSearch = new() { startNode };
        List<HexNode> processed = new();

        while (toSearch.Any())
        {
            HexNode current = toSearch[0];
            foreach (HexNode t in toSearch)
                if (t.F < current.F || t.F == current.F && t.H < current.H) current = t;

            processed.Add(current);
            toSearch.Remove(current);

            //When we find our target
            if (current == targetNode)
            {
                HexNode currentPathTile = targetNode;
                List<HexNode> path = new();
                var count = 100;
                while (currentPathTile != startNode)
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                    count--;
                    if (count < 0) throw new Exception();

                }
                path.Reverse();
                return path;
            }

            foreach (HexNode neighbor in current.Neighboors.Where(t => t.IsNodeWalkable() && !processed.Contains(t)))
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
                        neighbor.SetH(HexDistance.GetDistance(neighbor, targetNode));
                        toSearch.Add(neighbor);
                    }
                }
            }
        }

        Debug.Log("Error unreacheable");
        return null;

    }

    public override List<HexNode> Range(HexNode startNode, AbilityBase ability)
    {
        return BFS.TargTypeBFS(startNode, ability);
    }

}

