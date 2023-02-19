using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Rule Tile", menuName = "Tile/RuleTile")]
public class GameRuleTile : HexagonalRuleTile
{
    [SerializeField]
    private TileType type;
    public bool isWalkable;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

    }

        

}


[Serializable]
public enum TileType
{
    Grass,
    Water,
    Mountain
}