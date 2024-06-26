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
    private readonly NetworkVariable<bool> _surfaceWalkable = new(); //when set this triggers on all clients to update their surface value
    
    private SpriteRenderer _surfaceRenderer; //where the surface sprite is displayed
    private SpriteRenderer _hexRenderer; //where hex art is displayed

    [HideInInspector] public NetworkVariable<Vector3Int> GridPos = new(new Vector3Int(-100, -100, -100)); //set negative so that value will trigger on change
    [HideInInspector] public NetworkVariable<Vector3Int> CubeCoord = new(new Vector3Int(-100, -100, -100)); 

    private AbstractCharacter _characterOnNode;
    private readonly NetworkVariable<int> _characterOnNodeID = new(-1); //-1 because needs to detect change

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

    public override void OnNetworkSpawn()
    {
        GridPos.OnValueChanged += UpdateGridPos;
        CubeCoord.OnValueChanged += UpdateCubeCoord;
        _characterOnNodeID.OnValueChanged += SetCharacterOnNodeReference;
        _surfaceWalkable.OnValueChanged += UpdateSurfaceWalkable; //both server and client
        SurfaceName.OnValueChanged += UpdateSurfaceReference;

        if (!IsServer && IsClient) //Non host clients
        {
            _hexRenderer.sprite = _sprites[UnityEngine.Random.Range(0, _sprites.Count)];
        }
    }

    private void UpdateGridPos(Vector3Int prevVal, Vector3Int newVal)
    {
        GridManager.Instance.GridCoordTiles[newVal] = this;
        GridManager.Instance.DebugGrid.Add(this);
    }
    private void UpdateCubeCoord(Vector3Int prevVal, Vector3Int newVal)
    {
        GridManager.Instance.CubeCoordTiles[newVal] = this;
        GridManager.Instance.DebugCube.Add(this);
    }

    public override void OnNetworkDespawn()
    {
        GridPos.OnValueChanged -= UpdateGridPos;
        CubeCoord.OnValueChanged -= UpdateCubeCoord;
        _surfaceWalkable.OnValueChanged -= UpdateSurfaceWalkable;
        _characterOnNodeID.OnValueChanged -= SetCharacterOnNodeReference;
        SurfaceName.OnValueChanged -= UpdateSurfaceReference;
    }

    /// <summary>
    /// Whenever we change the surface string network variable we update the reference to the surface scriptable object
    /// </summary>
    /// <param name="prevVal"></param>
    /// <param name="newVal"></param>
    private void UpdateSurfaceReference(FixedString32Bytes prevVal,  FixedString32Bytes newVal)
    {
        if(_surface!= null)
        {
            Destroy(_surface);
        }
        _surface = Database.Instance.GetSurfaceByName(newVal.Value.Replace("(Clone)", "")); //Have to get rid of "clone" to make it work
        _surface.NodeOn = this;
        _surfaceRenderer.sprite = _surface.SurfaceSprite;
    }

    public void ServerInitHex(Vector3Int gridPos, Vector3Int cubePos, SurfaceBase surface)
    {
        GridPos.Value = gridPos;
        CubeCoord.Value = cubePos;
        SurfaceName.Value = surface.name;
        _hexRenderer.sprite = _sprites[UnityEngine.Random.Range(0, _sprites.Count)];
        SetSurfaceWalkable(surface.IsWalkable);
    }

    /// <summary>
    /// Sets the surface on the node
    /// </summary>
    /// <param name="surface">A surface to be put on node</param>
    public void SetSurface(SurfaceBase surface)
    {
        if (IsServer)
        {
            SurfaceName.Value = surface.name;
        }
        else
        {
            SetSurfaceServerRPC(surface.name);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void SetSurfaceServerRPC(string surfaceName)
    {
        SurfaceName.Value = surfaceName;
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
        if (IsServer) //Change net var for everyone
        {
            _surfaceWalkable.Value = isWalkable;
        }
        else //if not server send rpc to change variable
        {
            SurfaceWalkableServerRPC(isWalkable);
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void SurfaceWalkableServerRPC(bool isWalkable)
    {
        _surfaceWalkable.Value = isWalkable;
    }

    public void UpdateSurfaceWalkable(bool prevVal, bool newVal)
    {
        _surface.IsWalkable = newVal;
    }

    public void OnEnterSurface(AbstractCharacter character)
    {
        _surface.OnEnterNode(character);
    }

    private void SetCharacterOnNodeReference(int preVal, int newVal)
    {
        if(newVal < 0)
        {
            _characterOnNode = null;
        }
        else if(newVal >= 0)
        {
            _characterOnNode = Database.Instance.AbstractCharactersDB.Get(newVal);
        }
    }

    /// <summary>
    /// Sets the network variable which in turn will update the reference of character
    /// </summary>
    /// <param name="abstractCharacterID">If int is Less Than 0 will set to null</param>
    public void SetCharacterOnNode(int abstractCharacterID)
    {
        if (IsServer)
        {
            _characterOnNodeID.Value = abstractCharacterID;
        }
        else
        {
            SetCharacterOnNodeServerRPC(abstractCharacterID);
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetCharacterOnNodeServerRPC(int abstractCharacterID)
    {
        _characterOnNodeID.Value = abstractCharacterID;
    }

    public AbstractCharacter GetCharacterOnNode()
    {
        if (IsServer)
        {
            return _characterOnNode;
        }
        else
        {
            return Database.Instance.AbstractCharactersDB.Get(_characterOnNodeID.Value);
        }
    }

    //So we can globally know what player is hovering over
    private void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            MouseManager.Instance.NodeMouseIsOver = this;
            HighlightManager.Instance.HighlightHover(this, true);
        }
    }

    private void OnMouseExit()
    {
        HighlightManager.Instance.HighlightHover(this, false);
    }
}


