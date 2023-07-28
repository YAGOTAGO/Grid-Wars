using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BFS
{

    /// <summary>
    /// Does a BFS search of depth from a start node, highlights all nodes along the way on Moves Map
    /// </summary>
    /// <param name="startNode">Which node BFS starts at</param>
    /// <param name="depth">How far BFS should go</param>
    /// <param name="paintMoves">Whether or not to highlight Moves map with bfs result</param>
    /// <returns>A Set of walkable nodes that were found in BFS</returns>
    public static HashSet<HexNode> BFSWalkable(HexNode startNode, int depth)
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
            foreach (HexNode neighbor in curr.Neighboors.Where(t => t.IsNodeWalkable() && !visited.Contains(t)))
            {
                fronteir.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }

        return visited;

    }

    /// <summary>
    /// BFS checking for nodes that abilities can pass through
    /// </summary>
    /// <param name="startNode">Starting HexNode</param>
    /// <param name="depth">How far the range is</param>
    /// <returns>A set of nodes which abilities can pass through</returns>
    public static HashSet<HexNode> BFSAblitiesPassThrough(HexNode startNode, int depth)
    {

        HashSet<HexNode> visited = new();
        Queue<HexNode> fronteir = new();

        fronteir.Enqueue(startNode);
        visited.Add(startNode);

        while (fronteir.Count > 0)
        {
            HexNode curr = fronteir.Dequeue();

            int distance = HexDistance.GetDistance(startNode, curr);

            if (distance >= depth)
            {
                return visited;
            }

            //Each neighboor that abilities can passthrough and not visited
            foreach (HexNode neighbor in curr.Neighboors.Where(t => t.CanAbilitiesPassthrough() && !visited.Contains(t)))
            {
                fronteir.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }

        return visited;

    }
}

