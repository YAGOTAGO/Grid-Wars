using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFS
{

    /// <summary>
    /// Does a BFS search of depth from a start node, highlights all nodes along the way on Moves Map
    /// </summary>
    /// <param name="startNode">Which node BFS starts at</param>
    /// <param name="depth">How far BFS should go</param>
    /// <returns>A Set of walkable nodes that were found in BFS</returns>
    public static HashSet<HexNode> BFSvisited(HexNode startNode, int depth)
    {

        HashSet<HexNode> visited = new();
        Queue<HexNode> fronteir = new();

        fronteir.Enqueue(startNode);
        visited.Add(startNode);

        while (fronteir.Count>0)
        {
            HexNode curr = fronteir.Dequeue();

            int distance = HexDistance.GetDistance(startNode, curr);

            if(distance>= depth)
            {
                return visited;
            }

            //Each neighboor that is walkable and not visited
            foreach (HexNode neighbor in curr.Neighboors.Where(t => t.IsWalkable && !visited.Contains(t)))
            {
                fronteir.Enqueue(neighbor);
                visited.Add(neighbor);

                HighlightManager.Instance.MovesHighlight(neighbor.GridPos);
            }
        }

        return visited;

    }
}

