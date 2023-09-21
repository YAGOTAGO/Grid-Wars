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
    public Dictionary<Vector3Int, HexNode> CubeCoordTiles { get; private set; } = new();
    private Grid _grid; //used to put all tiles under
    private NetworkVariable<int> _tileNum = new(0); //used to yield

    [Header("Tile Prefabs")]
    private Dictionary<TileType, HexNode> _prefabDict;
    [SerializeField] private Tilemap _tileMap; //the map we will copy
    [SerializeField] private List<HexNode> _prefabs;
    
    public void Awake()
    {
        Instance = this;
        _grid = GetComponent<Grid>();
        
    }

    public void Start()
    {
        InitDict();

        if (IsServer)
        {
            InitBoard();
            InitNeighboors(); //caches the neighboors in each tile
        }
    }

    public override void OnNetworkSpawn()
    {
        if(!IsServer && IsClient) //Non server, client
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += ClientUpdateNeighboors;
        }
        
    }

    private void ClientUpdateNeighboors(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        StartCoroutine(WaitForGridCoord());
    }

    /// <summary>
    /// Waits until the grid has all the hexes in it
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitForGridCoord()
    {
        while(GridCoordTiles.Count < _tileNum.Value) //wait until all 
        {
            yield return null;
        }

        InitNeighboors();
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

                HexRuleTile tileInfo = _tileMap.GetTile<HexRuleTile>(position);
                
                SurfaceBase surface = Instantiate(tileInfo.Surface); //use instantiate so we use a copy

                //Instatiate the prefab
                HexNode tile = Instantiate(_prefabDict[tileInfo.Type], tileWorldPos, Quaternion.identity);
                tile.GetComponent<NetworkObject>().Spawn(); //spawn tile for the clients

                //Cache cube and grid pos
                Vector3Int cubePos = HexDistance.UnityCellToCube(position);//calculate cube pos
                tile.ServerInitHex(position, cubePos, surface); //Init Tile with grid and cube pos and the surface
                GridCoordTiles[position] = tile; //So we can lookup tile later from dict
                CubeCoordTiles[cubePos] = tile;

                //organizes look in editor
                tile.transform.SetParent(_grid.transform); 

                //Name to help debugging
                tile.name = tileInfo.Type.ToString() + _tileNum.Value;
                _tileNum.Value++;
            }

        }

    }


}
