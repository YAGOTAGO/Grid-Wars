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

    //Methods here return list of hexnodes that represent shape
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

public enum ShapeType
{
    Plus,
    Star,
    Circle,

}