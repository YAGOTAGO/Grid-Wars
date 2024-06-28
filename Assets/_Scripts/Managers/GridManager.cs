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

    //private
    private Grid _grid; //used to put all tiles under
    private int _tileNum; //used to know when to cache hex neighboors
    private Dictionary<TileType, HexNode> _prefabDict = new();
    
    [Header("Tile Prefabs")]
    [SerializeField] private Tilemap _tileMap; //the map we will copy
    [SerializeField] private List<HexNode> _prefabs;
    
    public void Awake()
    {
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

    private void WaitToInitHexNeighboors()
    {
        //Count num of tile in the tilemap
        foreach (Vector3Int position in _tileMap.cellBounds.allPositionsWithin)
        {
            if (_tileMap.HasTile(position))
            {
                _tileNum++;
            }
        }

        StartCoroutine(CacheNeighboors());
    }

    public IEnumerator CacheNeighboors()
    {
        yield return new WaitUntil(() => GridCoordTiles.Count >= _tileNum && CubeCoordTiles.Count >= _tileNum);
        MapLoaded = true;
        foreach (HexNode tile in GridCoordTiles.Values) tile.CacheNeighbors();
    }
    
    public void SpawnBoard(MapsBase map)
    {

        foreach (Vector3Int position in _tileMap.cellBounds.allPositionsWithin)
        {
            if (_tileMap.HasTile(position))
            {

                HexRuleTile tileInfo = _tileMap.GetTile<HexRuleTile>(position);
                SurfaceBase surface = Instantiate(tileInfo.Surface); //use instantiate so we use a copy

                HexNode tile = Instantiate(_prefabDict[tileInfo.Type], _tileMap.CellToWorld(position), Quaternion.identity);
                tile.GetComponent<NetworkObject>().Spawn(); //spawn tile for the clients

                tile.ServerInitHex(position, HexDistance.UnityCellToCube(position), surface); //Will set the data in Grid Manager

                //organizes look in editor
                tile.transform.SetParent(_grid.transform); 
            }

        }

    }


}
