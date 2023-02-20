using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Rule Tile", menuName = "Tile/RuleTile")]
public class GameRuleTile : HexagonalRuleTile
{   

    [SerializeField] public TileType type;


    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

    }

   

}

//Enums used to know what type the tile is
[Serializable]
public enum TileType
{
    Grass,
    Water,
    Mountain
}

