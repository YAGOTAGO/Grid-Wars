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
    private Grid _grid; //used to put all tiles under
    private readonly int _tileNum = 349; //number of tiles

    [Header("Tile Prefabs")]
    private Dictionary<TileType, HexNode> _prefabDict;
    [SerializeField] private Tilemap _tileMap; //the map we will copy
    [SerializeField] private List<HexNode> _prefabs;
    
    public void Awake()
    {
        Instance = this;
        _grid = GetComponent<Grid>();
        InitDict();
    }
    private void InitDict()
    {
        _prefabDict = new Dictionary<TileType, HexNode>();
        foreach (HexNode prefab in _prefabs)
        {
            _prefabDict[prefab.TileType] = prefab;
        }
    }

    /// <summary>
    /// Waits until the grid has all the hexes in it
    /// </summary>
    /// <returns></returns>
    public bool BoardLoad()
    {
        return GridCoordTiles.Count >= _tileNum && CubeCoordTiles.Count >= _tileNum;
    }

    public IEnumerator InitNeighboors()
    {
        yield return new WaitUntil(BoardLoad);
        if (IsClient && !IsServer) { GameManager.Instance.ChangeState(GameState.LoadCharacters); }
        foreach (HexNode tile in GridCoordTiles.Values) tile.CacheNeighbors();
    }
    
    public void InitBoard()
    {

        foreach (Vector3Int position in _tileMap.cellBounds.allPositionsWithin)
        {
            if (_tileMap.HasTile(position))
            {

                //Get position to instantiate at
                Vector3 tileWorldPos = _tileMap.CellToWorld(position);

                HexRuleTile tileInfo = _tileMap.GetTile<HexRuleTile>(position);
                
                SurfaceBase surface = Instantiate(tileInfo.Surface); //use instantiate so we use a copy

                //Instatiate the prefab
                HexNode tile = Instantiate(_prefabDict[tileInfo.Type], tileWorldPos, Quaternion.identity);
                tile.GetComponent<NetworkObject>().Spawn(); //spawn tile for the clients

                //Cache cube and grid pos
                Vector3Int cubePos = HexDistance.UnityCellToCube(position);//calculate cube pos
                tile.ServerInitHex(position, cubePos, surface); //Init Tile with grid and cube pos and the surface
                //GridCoordTiles[position] = tile; //So we can lookup tile later from dict
                //CubeCoordTiles[cubePos] = tile;

                //organizes look in editor
                tile.transform.SetParent(_grid.transform); 
            }

        }

    }


}
