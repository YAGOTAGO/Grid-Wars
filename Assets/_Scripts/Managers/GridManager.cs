using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class GridManager : NetworkBehaviour
{
    public static GridManager Instance;
    public Dictionary<Vector3Int, HexNode> GridCoordTiles { get; private set; } = new(); //know which tile by position
    public List<HexNode> DebugGrid = new();
    public Dictionary<Vector3Int, HexNode> CubeCoordTiles { get; private set; } = new();
    public List<HexNode> DebugCube = new();
    
    public bool MapLoaded { get; private set; } = false;
    [Header("Tile Prefabs")]
    [SerializeField] private List<HexNode> _prefabs;

    //private
    private Grid _grid; //used to put all tiles under
    private Dictionary<TileType, HexNode> _prefabDict = new();
    private Tilemap _tileMap;
    
    public void Awake()
    {
        Debug.Log("GRID AWAKE");
        Instance = this;
        _grid = GetComponent<Grid>();
        InitDict();
        //WaitToInitHexNeighboors();
    }
    private void InitDict()
    {
        foreach (HexNode prefab in _prefabs)
        {
            _prefabDict[prefab.TileType] = prefab;
        }
    }

    public IEnumerator CacheNeighboors(int tileNum)
    {
        yield return new WaitUntil(() => GridCoordTiles.Count >= tileNum && CubeCoordTiles.Count >= tileNum);
        MapLoaded = true;
        foreach (HexNode tile in GridCoordTiles.Values) tile.CacheNeighbors();
    }
    
    public void SpawnBoard(MapsBase map)
    {
        Debug.Log("Spawning Board");

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
