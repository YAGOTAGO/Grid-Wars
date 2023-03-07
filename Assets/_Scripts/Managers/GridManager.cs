using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public Dictionary<Vector3Int, HexNode> GridCoordTiles { get; private set; } //know which tile by position
    public Dictionary<Vector3Int, HexNode> CubeCoordTiles { get; private set; }
    public Grid Grid { get; private set; } //used to put all tiles under

    #region TilePrefabs

    //Could make a tile prefab scrip that hold a dictionary of them based on the enum

    [Header("Tile Prefabs")]
    private Dictionary<TileType, HexNode> _prefabDict;
    [SerializeField] private Tilemap _tileMap; //the map we will copy
    [SerializeField] private List<HexNode> _prefabs;
    
    #endregion
    
    void Awake()
    {
        InitVars();
        InitDict();
        InitBoard();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        InitNeighboors(); //caches the neighboors in each tile
    }

    private void Update()
    {   
        Highlight();
    }

    private void InitVars()
    {
        Instance = this;
        GridCoordTiles = new();
        CubeCoordTiles = new();
        Grid = this.GetComponent<Grid>();
    }

    private void InitNeighboors()
    {
        foreach (HexNode tile in GridCoordTiles.Values) tile.CacheNeighbors();
    }
        
    //Highlights on hover
    private void Highlight()
    {
        if (GridCoordTiles.ContainsKey(MouseManager.Instance.MouseCellPos))
        {
            HighlightManager.Instance.HoverHighlight(MouseManager.Instance.MouseCellPos);
        }
    }

    private void InitDict()
    {
        _prefabDict = new Dictionary<TileType, HexNode>();
        foreach (HexNode prefab in _prefabs)
        {
            _prefabDict[prefab.GetTileType()] = prefab;
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

                TileType type = _tileMap.GetTile<GameRuleTile>(position).type;

                //Instatiate the prefab
                HexNode tile = Instantiate(_prefabDict[type], tileWorldPos, Quaternion.identity);

                //Cache cube and grid pos
                Vector3Int cubePos = HexDistance.UnityCellToCube(position);//calculate cube pos
                tile.Init(position, cubePos); //Init Tile with grid and cube pos
                GridCoordTiles[position] = tile; //So we can lookup tile later from dict
                CubeCoordTiles[cubePos] = tile;

                //organizes look in editor
                tile.transform.SetParent(Grid.transform); 
            }

        }

        //Destroy tilemap here prolly?, since should not need it again
    }

}
