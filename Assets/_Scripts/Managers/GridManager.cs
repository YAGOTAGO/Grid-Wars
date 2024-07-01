using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : NetworkBehaviour
{
    public static GridManager Instance;
    public Dictionary<Vector3Int, HexNode> GridCoordTiles { get; private set; } = new(); //know which tile by position
    public List<HexNode> DebugGrid = new();
    public Dictionary<Vector3Int, HexNode> CubeCoordTiles { get; private set; } = new();
    public List<HexNode> DebugCube = new();
    
    [Header("Tile Prefabs")]
    [SerializeField] private List<HexNode> _prefabs;

    //private
    private Dictionary<TileType, HexNode> _prefabDict = new();
    
    public void Awake()
    {
        Instance = this;
        InitDict();
    }
    private void InitDict()
    {
        foreach (HexNode prefab in _prefabs)
        {
            _prefabDict[prefab.TileType] = prefab;
        }
    }

    public void CacheNeighbors()
    {
        foreach (HexNode tile in GridCoordTiles.Values) tile.CacheNeighbors();
    }
    
    public void SpawnBoard(MapsBase map)
    {
        Tilemap _tileMap = Instantiate(map.TileMap, transform);

        foreach (Vector3Int position in _tileMap.cellBounds.allPositionsWithin)
        {
            if (_tileMap.HasTile(position))
            {

                HexRuleTile tileInfo = _tileMap.GetTile<HexRuleTile>(position);
                SurfaceBase surface = Instantiate(tileInfo.Surface); //use instantiate so we use a copy

                HexNode tile = Instantiate(_prefabDict[tileInfo.Type], _tileMap.CellToWorld(position), Quaternion.identity);
                tile.GetComponent<NetworkObject>().Spawn(); //spawn tile for the clients

                tile.ServerInitHex(position, HexDistance.UnityCellToCube(position), surface); //Will set the data in Grid Manager
            }

        }

    }


}
