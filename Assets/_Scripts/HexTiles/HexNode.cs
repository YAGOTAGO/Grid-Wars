using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof (SpriteRenderer))]
public class HexNode : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private List<Sprite> _sprites;
    public bool IsWalkable; //Whether characters can be on it
    public bool IsPassable; //Whether abilies can pass over
    [SerializeField] private TileType TileType; //HAS TO BE PUBLIC TO BE SET IN EDITOR
    private Character characterOnNode;
        
    [HideInInspector] public Vector3Int GridPos { get; private set; } //Unity grid x, y, z
    [HideInInspector] public Vector3Int CubeCoord { get; private set; } //Unity grid converted into cube coords

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
        _renderer = this.GetComponent<SpriteRenderer>();
    }

    //Inits the Hex
    public void Init(Vector3Int gridPos, Vector3Int cubePos)
    {
        GridPos = gridPos;
        CubeCoord = cubePos;
        _renderer.sprite = _sprites[UnityEngine.Random.Range(0, _sprites.Count())];
    }

    public TileType GetTileType()
    {
        return TileType;
    }

    public Character GetCharacter()
    {
        return characterOnNode;
    }

    public void SetCharacter(Character character)
    {
        characterOnNode = character;
    }

    
}


