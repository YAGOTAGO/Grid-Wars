using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFS
{

    //Returns the list of visited nodes within the depth
    public static HashSet<HexNode> BFSvisited(HexNode startNode, int depth)
    {
        Debug.Log("BFS CALLED");

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
            foreach (HexNode neighbor in curr.Neighboors.Where(t => t.isWalkable && !visited.Contains(t)))
            {
                fronteir.Enqueue(neighbor);
                visited.Add(neighbor);

                HighlightManager.Instance.MovesHighlight(neighbor.GridPos);
            }
        }

        return visited;

    }
}

