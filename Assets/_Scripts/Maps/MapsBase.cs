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
}
