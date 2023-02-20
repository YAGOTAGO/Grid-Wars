using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PathFinding
{
   /* public static List<GameRuleTile> FindPath(GameRuleTile startNode, GameRuleTile targetNode)
    {
        var toSearch = new List<GameRuleTile>() { startNode };
        var processed = new List<GameRuleTile>();

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

    }*/


}
