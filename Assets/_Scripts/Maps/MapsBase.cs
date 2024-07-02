using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Map", menuName = "Map")]
public class MapsBase : ScriptableObject
{
    [Header("Tile Map")]
    public Tilemap TileMap;
    public string MapName;
    [Range(2,3)]public int NumOfCharacters;

    [Header("Spawn Points")]
    public List<Vector3Int> SpawnPosServer = new();
    public List<Vector3Int> SpawnPosClient = new();

    public int GetNumTiles()
    {
        int num = 0;
        foreach (Vector3Int position in TileMap.cellBounds.allPositionsWithin)
        {
            if (TileMap.HasTile(position))
            {
                num++;
            }
        }
        return num;
    }
}
