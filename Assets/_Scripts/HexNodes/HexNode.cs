using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

[RequireComponent(typeof (SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class HexNode : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private List<Sprite> _sprites;
    public TileType TileType;
    
    private SurfaceBase _surface;
    public NetworkVariable<FixedString32Bytes> SurfaceName = new(); //used to know what surface we hold
    private SpriteRenderer _surfaceRenderer; //where the surface sprite is displayed
    private SpriteRenderer _hexRenderer; //where hex art is displayed

    [HideInInspector] public NetworkVariable<Vector3Int> GridPos { get; private set; } = new();//Unity grid x, y, z
    [HideInInspector] public NetworkVariable<Vector3Int> CubeCoord { get; private set; } = new();//Unity grid converted into cube coords

    public AbstractCharacter CharacterOnNode;

    #region Pathfinding
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public List<HexNode> Neighboors { get; protected set; }
    public HexNode Connection { get; private set; }

    public void SetConnection(HexNode node)
    {
        Connection = node;
    }

    public void CacheNeighbors()
    {   
        Neighboors = GridManager.Instance.GridCoordTiles.Where(t => HexDistance.GetDistance(this, t.Value) == 1).Select(t => t.Value).ToList();
    
    }

    public void SetG(float g){G = g;}

    public void SetH(float h){H = h;}

    #endregion

    private void Awake()
    {
        _hexRenderer = GetComponent<SpriteRenderer>();
        _surfaceRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Updates the grid manager with hexnodes for the client
    /// </summary>
    private void ClientUpdateGridManager(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        GridManager.Instance.GridCoordTiles[GridPos.Value] = this;
        GridManager.Instance.CubeCoordTiles[CubeCoord.Value] = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(!IsServer && IsClient)
        {
            SurfaceName.OnValueChanged += UpdateTheSurfaceSO;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += ClientUpdateGridManager;
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        if (!IsServer && IsClient)
        {
            SurfaceName.OnValueChanged -= UpdateTheSurfaceSO;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= ClientUpdateGridManager;
        }
    }

    /// <summary>
    /// Whenever we change the surface string network variable we update the reference to the surface scriptable object
    /// </summary>
    /// <param name="prevVal"></param>
    /// <param name="newVal"></param>
    private void UpdateTheSurfaceSO(FixedString32Bytes prevVal,  FixedString32Bytes newVal)
    {
        SetSurface(Database.Instance.GetSurface(newVal.Value.Replace("(Clone)", ""))); //Have to get rid of "clone" to make it work
    }

    //Inits the Hex
    public void ServerInitHex(Vector3Int gridPos, Vector3Int cubePos, SurfaceBase surface)
    {
        GridPos.Value = gridPos;
        CubeCoord.Value = cubePos;
        SurfaceName.Value = surface.name;
        _hexRenderer.sprite = _sprites[UnityEngine.Random.Range(0, _sprites.Count)];
        SetSurface(surface);
    }

    /// <summary>
    /// Sets the surface on the node
    /// </summary>
    /// <param name="surface">A surface to be put on node</param>
    public void SetSurface(SurfaceBase surface)
    {
        _surface = surface;
        _surfaceRenderer.sprite = surface.SurfaceSprite;
    }

    public bool IsNodeWalkable()
    {
        return _surface.IsWalkable;
    }

    public bool CanAbilitiesPassthrough()
    {
        return _surface.CanAbilitiesPassthrough;
    }

    public void SetSurfaceWalkable(bool isWalkable)
    {
        _surface.IsWalkable = isWalkable;
    }

    public void OnEnterSurface(AbstractCharacter character)
    {
        _surface.OnTouchNode(character);
    }

    //So we can globally know what player is hovering over
    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            MouseManager.Instance.NodeMouseIsOver = this;
        }
    }
}


