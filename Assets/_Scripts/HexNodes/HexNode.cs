using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof (SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class HexNode : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Sprite> _sprites;
    public TileType TileType;
    
    [Header("Surface")]
    private SurfaceBase _surface;
    private SpriteRenderer _surfaceRenderer;

    [HideInInspector] public Vector3Int GridPos { get; private set; } //Unity grid x, y, z
    [HideInInspector] public Vector3Int CubeCoord { get; private set; } //Unity grid converted into cube coords
    
    private SpriteRenderer _renderer;
    [HideInInspector]public AbstractCharacter CharacterOnNode;

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
        _renderer = GetComponent<SpriteRenderer>();
        _surfaceRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    //Inits the Hex
    public void Init(Vector3Int gridPos, Vector3Int cubePos)
    {
        GridPos = gridPos;
        CubeCoord = cubePos;
        _renderer.sprite = _sprites[UnityEngine.Random.Range(0, _sprites.Count)];
        InitSurface();
    }

    private void InitSurface()
    {
        switch (TileType)
        {
            case TileType.Grass:
                SetSurface(Database.Instance.GetSurface("CommonRewardSurface")); 
                break;
            case TileType.Mountain:
                SetSurface(Database.Instance.GetSurface("CommonRewardSurface"));
                break;
            case TileType.Water:
                SetSurface(Database.Instance.GetSurface("CommonRewardSurface"));
                break;
        }
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


