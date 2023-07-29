using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding
{
    public static List<HexNode> FindPath(HexNode startNode, HexNode targetNode)
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
        
    public static List<HexNode> FindPathBreakpoints(HexNode startNode, HexNode targetNode, List<HexNode> breakpoints)
    {
        HashSet<HexNode> toSearch = new() { startNode };
        HashSet<HexNode> processed = new();

        while (toSearch.Count > 0)
        {
            HexNode current = toSearch.OrderBy(node => node.F).ThenBy(node => node.H).First();
            toSearch.Remove(current);
            processed.Add(current);

            // When we find our target
            if (current == targetNode)
            {
                List<HexNode> path = new List<HexNode>();
                HexNode currentPathTile = targetNode;

                while (currentPathTile != startNode)
                {
                    path.Add(currentPathTile);
                    currentPathTile = currentPathTile.Connection;
                }

                path.Reverse();

                // Insert breakpoints into the path list at the desired positions
                foreach (var breakpoint in breakpoints)
                {
                    int index = path.FindIndex(node => node == breakpoint);
                    if (index != -1)
                    {
                        path.Insert(index, breakpoint);
                    }
                }

                return path;
            }

            foreach (HexNode neighbor in current.Neighboors.Where(node => node.IsNodeWalkable() && !processed.Contains(node)))
            {
                var costToNeighbor = current.G + 1;

                if (!toSearch.Contains(neighbor) || costToNeighbor < neighbor.G)
                {
                    neighbor.SetG(costToNeighbor);
                    neighbor.SetH(HexDistance.GetDistance(neighbor, targetNode));
                    neighbor.SetConnection(current);

                    if (!toSearch.Contains(neighbor))
                    {
                        toSearch.Add(neighbor);
                    }
                }
            }
        }

        Debug.Log("Error: Unreachable");
        return null;
    }
}
