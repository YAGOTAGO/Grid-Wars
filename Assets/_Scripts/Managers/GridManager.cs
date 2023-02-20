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

    [SerializeField] private Highlight highlightScript;
    private Vector3Int cellPos;

    #region TilePrefabs
    [Header("Tile Prefabs")]
    [SerializeField] private Tilemap tileMap; //the map we will copy
    [SerializeField] private HexNode _grassPrefab;
    [SerializeField] private HexNode _mountainPrefab;
    [SerializeField] private HexNode _waterPrefab;
    #endregion

    void Awake()
    {
        Instance = this;
        tilesDict = new Dictionary<Vector3Int, HexNode>();
        grid = this.GetComponent<Grid>();
        cellPos = new Vector3Int();
    }

    void Start()
    {
        InitBoard();

    }

    private void Update()
    {   

        cellPos = MouseInput.Instance.GetCellPosFromMouse(grid);

        if (tilesDict.ContainsKey(cellPos))
        {
            highlightScript.HighlightTiles(cellPos);
        }
    }

    private void InitBoard()
    {
        foreach (Vector3Int position in tileMap.cellBounds.allPositionsWithin)
        {
            if (tileMap.HasTile(position))
            {

                //Get position to instantiate at
                Vector3 tileWorldPos = tileMap.CellToWorld(position);

                TileType type = tileMap.GetTile<GameRuleTile>(position).type;
                HexNode tile = null;

                switch (type)
                {
                    case TileType.Grass: tile = Instantiate(_grassPrefab, tileWorldPos, Quaternion.identity); break;

                    case TileType.Mountain: tile = Instantiate(_mountainPrefab, tileWorldPos, Quaternion.identity); break;

                    case TileType.Water: tile = Instantiate(_waterPrefab, tileWorldPos, Quaternion.identity); break;

                    default: Debug.Log("Error tried to load tile that didnt match type in GridManager"); break;
                }

                tile.transform.SetParent(grid.transform); //organizes look in editor
                tile.Init(position); //so tile knows own position
                tilesDict[position] = tile; //So we can lookup tile later from dict

            }

        }

        //Destory tilemap here prolly, since should not need it again
    }

}
