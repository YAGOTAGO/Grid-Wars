using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexDistance
{
    //Returns distance between two nodes
    public static int GetDistance(HexNode A, HexNode B)
    {
        return CubeDistance(A.cubeCoord, B.cubeCoord);
    }

    public static Vector3Int UnityCellToCube(Vector3Int cell)
    {
        var yCell = cell.x;
        var xCell = cell.y;
        var x = yCell - (xCell - (xCell & 1)) / 2;
        var z = xCell;
        var y = -x - z;
        return new Vector3Int(x, y, z);
    }

    private static Vector3Int CubeSubtract(Vector3Int A, Vector3Int B)
    {
        return new Vector3Int(A.x - B.x, A.y - B.y, A.z - B.z);
    }

    private static int CubeDistance(Vector3Int A, Vector3Int B)
    {
        Vector3Int vec = CubeSubtract(A, B);
        return (Math.Abs(vec.x) + Math.Abs(vec.y) + Math.Abs(vec.z)) / 2;
    }
}
