using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public Dictionary<Vector3Int, HexNode> GridCoordTiles { get; private set; } = new(); //know which tile by position
    public Dictionary<Vector3Int, HexNode> CubeCoordTiles { get; private set; } = new();
    public Grid Grid { get; private set; } //used to put all tiles under
    private int _tileNum = 0; //to name tiles in editor

    #region TilePrefabs

    //Could make a tile prefab scrip that hold a dictionary of them based on the enum

    [Header("Tile Prefabs")]
    private Dictionary<TileType, HexNode> _prefabDict;
    [SerializeField] private Tilemap _tileMap; //the map we will copy
    [SerializeField] private List<HexNode> _prefabs;
    
    #endregion
    
    void Awake()
    {
        Instance = this;
        Grid = GetComponent<Grid>();
        
    }

    void Start()
    {
        InitDict();
        InitBoard();
        InitNeighboors(); //caches the neighboors in each tile
    }

    private void InitNeighboors()
    {
        foreach (HexNode tile in GridCoordTiles.Values) tile.CacheNeighbors();
    }
    
    private void InitDict()
    {
        _prefabDict = new Dictionary<TileType, HexNode>();
        foreach (HexNode prefab in _prefabs)
        {
            _prefabDict[prefab.TileType] = prefab;
        }

    }
    private void InitBoard()
    {
        foreach (Vector3Int position in _tileMap.cellBounds.allPositionsWithin)
        {
            if (_tileMap.HasTile(position))
            {

                //Get position to instantiate at
                Vector3 tileWorldPos = _tileMap.CellToWorld(position);

                GameRuleTile tileInfo = _tileMap.GetTile<GameRuleTile>(position);
                
                SurfaceBase surface = Instantiate(tileInfo.Surface); //use instantiate so we use a copy

                //Instatiate the prefab
                HexNode tile = Instantiate(_prefabDict[tileInfo.Type], tileWorldPos, Quaternion.identity);
                
                //Cache cube and grid pos
                Vector3Int cubePos = HexDistance.UnityCellToCube(position);//calculate cube pos
                tile.Init(position, cubePos, surface); //Init Tile with grid and cube pos and the surface
                GridCoordTiles[position] = tile; //So we can lookup tile later from dict
                CubeCoordTiles[cubePos] = tile;

                //organizes look in editor
                tile.transform.SetParent(Grid.transform); 

                //Name to help debugging
                tile.name = tileInfo.Type.ToString() + _tileNum;
                _tileNum++;
            }

        }

        //Destroy tilemap here prolly?, since should not need it again
    }


}
