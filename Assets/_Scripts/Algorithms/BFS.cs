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
    /// <param name="paintMoves">Whether or not to highlight Moves map with bfs result</param>
    /// <returns>A Set of walkable nodes that were found in BFS</returns>
    public static List<HexNode> BFSWalkable(HexNode startNode, int depth)
    {

        List<HexNode> visited = new();
        Queue<HexNode> fronteir = new();

        fronteir.Enqueue(startNode);
        visited.Add(startNode);

        while (fronteir.Count>0)
        {
            HexNode curr = fronteir.Dequeue();

            int distance = HexDistance.GetDistance(startNode, curr);

            if(distance>= depth)
            {
                visited.Remove(startNode);
                return visited;
            }

            //Each neighboor that is walkable and not visited
            foreach (HexNode neighbor in curr.Neighboors.Where(t => t.IsNodeWalkable() && !visited.Contains(t)))
            {
                fronteir.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }
        visited.Remove(startNode);
        return visited;

    }

    /// <summary>
    /// BFS checking for nodes that abilities can pass through
    /// </summary>
    /// <param name="startNode">Starting HexNode</param>
    /// <param name="depth">How far the range is</param>
    /// <returns>A set of nodes which abilities can pass through</returns>
    public static List<HexNode> BFSNormal(HexNode startNode, int depth)
    {

        List<HexNode> visited = new();
        Queue<HexNode> fronteir = new();

        fronteir.Enqueue(startNode);
        visited.Add(startNode);

        while (fronteir.Count > 0)
        {
            HexNode curr = fronteir.Dequeue();

            int distance = HexDistance.GetDistance(startNode, curr);

            if (distance >= depth)
            {
                visited.Remove(startNode);
                return visited;
            }

            //Each neighboor that abilities can passthrough and not visited
            foreach (HexNode neighbor in curr.Neighboors.Where(t => t.CanAbilitiesPassthrough() && !visited.Contains(t)))
            {
                fronteir.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }

        visited.Remove(startNode);
        return visited;

    }

    /// <summary>
    /// A bfs that gives all nodes in given range
    /// </summary>
    /// <param name="startNode">Start Node</param>
    /// <param name="depth"> Depth of BFS</param>
    /// <returns>A set of nodes that are in depth</returns>
    public static List<HexNode> BFSAireal(HexNode startNode, int depth)
    {

        List<HexNode> visited = new();
        Queue<HexNode> fronteir = new();

        fronteir.Enqueue(startNode);
        //visited.Add(startNode);

        while (fronteir.Count > 0)
        {
            HexNode curr = fronteir.Dequeue();

            int distance = HexDistance.GetDistance(startNode, curr);

            if (distance >= depth)
            {
                //visited.Remove(startNode);
                return visited;
            }

            //Each neighboor that abilities can passthrough and not visited
            foreach (HexNode neighbor in curr.Neighboors.Where(t => !visited.Contains(t)))
            {
                fronteir.Enqueue(neighbor);
                visited.Add(neighbor);
            }
        }

        //visited.Remove(startNode);
        return visited;

    }

    /// <summary>
    /// Chooses BFS by targeting type
    /// </summary>
    /// <param name="startNode"></param>
    /// <param name="ability"></param>
    /// <returns></returns>
    public static List<HexNode> TargTypeBFS(HexNode startNode, AbilityBase ability)
    {

        if (ability.GetTargetingType() == TargetingType.WALKABLE)
        {
            return BFSWalkable(startNode, ability.Range);
        }

        if (ability.GetTargetingType() == TargetingType.NORMAL)
        {
            return BFSNormal(startNode, ability.Range);
        }

        if (ability.GetTargetingType() == TargetingType.AIREAL)
        {
            return BFSAireal(startNode, ability.Range);
        }

        Debug.LogWarning("Didnt fing targeting type in TargTypeBFS.");
        return null;
    }



}

