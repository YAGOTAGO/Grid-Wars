using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rule Tile", menuName = "Tile/HexRuleTile")]
public class HexRuleTile : HexagonalRuleTile
{   

    public TileType Type;
    public SurfaceBase Surface;
}


