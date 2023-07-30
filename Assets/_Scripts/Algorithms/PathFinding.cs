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
        if (breakpoints.Count == 0) { return FindPath(startNode, targetNode); }

        List<HexNode> pathToFirstBreakpoint = FindPath(startNode, breakpoints[0]);
        HexNode currentBreakpoint = breakpoints[0];

        for (int i = 1; i < breakpoints.Count; i++)
        {
            List<HexNode> tempPath = FindPath(currentBreakpoint, breakpoints[i]);
            pathToFirstBreakpoint.AddRange(tempPath.GetRange(1, tempPath.Count - 1));
            currentBreakpoint = breakpoints[i];
        }

        List<HexNode> pathFromLastBreakpointToTarget = FindPath(currentBreakpoint, targetNode);
        pathToFirstBreakpoint.AddRange(pathFromLastBreakpointToTarget.GetRange(1, pathFromLastBreakpointToTarget.Count - 1));

        return pathToFirstBreakpoint;
    }
}
