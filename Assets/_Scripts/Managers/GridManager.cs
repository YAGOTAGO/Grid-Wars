using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public Dictionary<Vector3Int, HexNode> tilesDict { get; private set; } //know which tile by position
    public Grid grid { get; private set; } //used to put all tiles under

    public Vector3Int cellPos { get; private set; }

    #region TilePrefabs

    //Could make a tile prefab scrip that hold a dictionary of them based on the enum

    [Header("Tile Prefabs")]
    private Dictionary<TileType, HexNode> _prefabDict;
    [SerializeField] private Tilemap _tileMap; //the map we will copy
    [SerializeField] private List<HexNode> _prefabs;
    /*[SerializeField] private HexNode _grassPrefab;
    [SerializeField] private HexNode _mountainPrefab;
    [SerializeField] private HexNode _waterPrefab;*/
    #endregion
    
    void Awake()
    {
        Instance = this;
        tilesDict = new Dictionary<Vector3Int, HexNode>();
        _prefabDict = new Dictionary<TileType, HexNode>();
        grid = this.GetComponent<Grid>();
        cellPos = new Vector3Int();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        InitDict();
        InitBoard();
        InitNeighboors(); //caches the neighboors in each tile
    }

    private void Update()
    {   
        cellPos = MouseManager.Instance.GetCellPosFromMouse();
        Highlight(cellPos);
    }

    private void InitNeighboors()
    {
        foreach (HexNode tile in tilesDict.Values) tile.CacheNeighbors();
    }

    //Highlights on hover
    private void Highlight(Vector3Int cell)
    {
        if (tilesDict.ContainsKey(cellPos))
        {
            HighlightManager.Instance.HoverHighlight(cellPos);
        }
    }

    private void InitDict()
    {
        foreach(HexNode prefab in _prefabs)
        {
            _prefabDict[prefab.tileType] = prefab;
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

                tile.transform.SetParent(grid.transform); //organizes look in editor
                tile.Init(position); //so tile knows own position
                tilesDict[position] = tile; //So we can lookup tile later from dict
                
            }

        }

        //Destory tilemap here prolly, since should not need it again
    }

}
